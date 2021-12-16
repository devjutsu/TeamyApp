using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Models.Polls;
using Teamy.Server.Services;
using Teamy.Shared.Common;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollsController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        UserManager<AppUser> _userManager { get; set; }
        readonly IMapper _mapper;
        IVoteHub _hub;
        public PollsController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    UserManager<AppUser> userManager,
                                    IMapper mapper,
                                    IVoteHub hub)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _hub = hub;
        }

        [HttpPost("Vote")]
        public async Task<IActionResult> Vote([FromBody] PollChoiceVM choice)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var poll = await _db.Polls.FindAsync(choice.PollId);
            var answer = new PollAnswer() { PollChoiceId = choice.Id.Value, UserId = currentUserId };

            if (poll.MultiChoice)
            {
                var myAnswerToDelete = await _db.PollAnswers.Where(o => o.PollChoiceId == choice.Id.Value && o.UserId == currentUserId).FirstOrDefaultAsync();
                if (myAnswerToDelete != null)
                {
                    _db.PollAnswers.Remove(myAnswerToDelete);
                }
                else
                {
                    _db.PollAnswers.Add(answer);
                }
            }
            else
            {
                var myAnswers = await _db.PollAnswers
                            .Include(o => o.PollChoice)
                            .Where(o => o.PollChoice.PollId == choice.PollId && o.UserId == currentUserId)
                            .ToListAsync();

                _db.PollAnswers.RemoveRange(myAnswers);
                _db.PollAnswers.Add(answer);
            }

            await _db.SaveChangesAsync();
            await _hub.EventUpdated(poll.EventId);

            return Ok();
        }

        [HttpPost("Reset")]
        public async Task Reset([FromBody] PollVM pollVM)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var answers = _db.PollChoices.Include(_ => _.Answers)
                            .Where(_ => _.PollId == pollVM.Id)
                            .SelectMany(c => c.Answers.Where(a => a.UserId == currentUserId));

            _db.PollAnswers.RemoveRange(answers);
            await _db.SaveChangesAsync();
            await _hub.EventUpdated(pollVM.EventId.Value);
        }

        [HttpPost("VoteDate")]
        public async Task<IActionResult> VoteDate([FromBody] ProposedDateVM date)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var proposed = await _db.Events
                                    .Include(o => o.ProposedDates)
                                    .ThenInclude(o => o.Votes)
                                    .Where(o => o.Id == date.EventId)
                                    .FirstAsync();

            var alradyVoted = proposed.ProposedDates.SelectMany(o => o.Votes).Where(o => o.UserId == currentUserId && date.Id == o.ProposedDateId);

            if (alradyVoted.Count() > 0)
            {
                _db.DateVotes.RemoveRange(alradyVoted);
            }
            else
            {
                _db.DateVotes.Add(new DateVote() { UserId = currentUserId, ProposedDateId = date.Id });
            }

            await _db.SaveChangesAsync();

            await _hub.EventUpdated(date.EventId.Value);
            return Ok();
        }

        [HttpPost("LockDate")]
        public async Task<IActionResult> LockDate([FromBody] ProposedDateVM date)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var evt = await _db.Events.Where(o => o.Id == date.EventId && o.CreatedById == currentUserId).FirstAsync();

            evt.EventDate = date.Date;
            evt.EventDateTo = date.DateTo;
            evt.DateStatus = EventDateStatus.Locked;

            _db.Events.Update(evt);
            await _db.SaveChangesAsync();

            await _hub.EventUpdated(date.EventId.Value);
            return Ok();
        }

        [HttpPost("UnlockDate")]
        public async Task<IActionResult> UnlockDate(ProposedDateVM date)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var evt = await _db.Events.Where(o => o.Id == date.EventId && o.CreatedById == currentUserId).FirstAsync();

            evt.EventDate = null;
            evt.EventDateTo = null;
            evt.DateStatus = EventDateStatus.Voting;

            _db.Events.Update(evt);
            await _db.SaveChangesAsync();

            await _hub.EventUpdated(date.EventId.Value);
            return Ok();
        }
    }
}