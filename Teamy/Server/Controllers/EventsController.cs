using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Services;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        UserManager<AppUser> _userManager { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        public EventsController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    UserManager<AppUser> userManager,
                                    IMapper mapper,
                                    IVoteHub hub)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EventVM eventVM)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;

                var evt = _mapper.Map<Event>(eventVM);
                evt.CreatedById = currentUserId;
                evt.CreatedBy = await _db.Users.FindAsync(currentUserId);
                evt.Invites = new List<Invite>() { new Invite() { InviteCode = GenerateInvite(), InvitedById = currentUserId, Public = true } };
                if (string.IsNullOrEmpty(eventVM.ImageUrl))
                    evt.CoverImage = null;

                var addedEvt = _db.Events.Add(evt);
                await _db.SaveChangesAsync();
                return Ok($"{addedEvt.Entity.Id}");
            }
            catch (Exception ex)
            { throw; }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] EventVM eventVM)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;

                var existingEvent = _db.Events
                                    .Include(_ => _.Polls)
                                    .ThenInclude(_ => _.Choices)
                                    .ThenInclude(_ => _.Answers)
                                    .Include(_ => _.CoverImage)
                                    .First(_ => _.Id == eventVM.Id);

                var newEvent = _mapper.Map<Event>(eventVM);
                if (existingEvent.CoverImage?.Url != eventVM.ImageUrl)
                {
                    if (eventVM.ImageUrl == null)
                        existingEvent.CoverImageId = null;
                    else
                        existingEvent.CoverImage = new ImageModel() { Url = eventVM.ImageUrl };
                }
                existingEvent.Polls = newEvent.Polls;
                existingEvent.ProposedDates = newEvent.ProposedDates;
                existingEvent.Title = eventVM.Title;
                existingEvent.Description = eventVM.Description;
                existingEvent.EventDate = eventVM.EventDate;
                existingEvent.EventDateTo = eventVM.EventDateTo;
                existingEvent.Where = eventVM.Where;
                existingEvent.DateStatus = eventVM.DateStatus;

                var existingProposedDates = _db.ProposedDates.Where(o => o.EventId == existingEvent.Id);
                _db.ProposedDates.RemoveRange(existingProposedDates);

                var e = _db.Events.Update(existingEvent);
                await _db.SaveChangesAsync();
                await _hub.EventUpdated(e.Entity.Id);
                return Ok($"{e.Entity.Id}");
            }
            catch (Exception ex)
            { throw; }
        }

        [HttpGet("Upcoming/{count}")]
        public List<EventVM> Upcoming()
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var events = _db.Events
                                .Include(_ => _.Participants)
                                .ThenInclude(z => z.User)
                                .Include(_ => _.Polls)
                                .ThenInclude(_ => _.Choices)
                                .ThenInclude(_ => _.Answers)
                                .Include(_ => _.Invites)
                                .Include(_ => _.CoverImage)
                                .Include(_ => _.CreatedBy)
                                .Include(_ => _.ProposedDates)
                                .ThenInclude(_ => _.Votes)
                                .Where(_ => _.CreatedById == currentUserId || _.Participants.Any(p => p.UserId == currentUserId))
                                .OrderBy(_ => _.EventDate)
                                .ToList();

                var vms = _mapper.Map<List<Event>, List<EventVM>>(events);
                return vms;
            }
            catch (Exception ex)
            { throw; }
        }

        [HttpGet("Get/{id}")]
        public async Task<EventVM> Get(Guid id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                var evt = await _db.Events
                                .Include(_ => _.Participants)
                                .ThenInclude(z => z.User)
                                .Include(_ => _.Polls)
                                .ThenInclude(_ => _.Choices)
                                .ThenInclude(_ => _.Answers)
                                .Include(_ => _.Invites)
                                .Include(_ => _.CoverImage)
                                .Include(_ => _.CreatedBy)
                                .Include(_ => _.ProposedDates)
                                .ThenInclude(_ => _.Votes)
                                .Where(_ => _.CreatedById == currentUserId || _.Participants.Any(p => p.UserId == currentUserId))
                                .FirstAsync(o => o.Id == id);

                var vm = _mapper.Map<EventVM>(evt);
                return vm;
            }
            catch (Exception ex)
            { throw; }
        }

        [HttpGet("Invited")]
        public async Task<EventVM> Invited([FromBody] string inviteCode)
        {
            try
            {
                var evt = await _db.Events
                                .Include(_ => _.Participants)
                                .ThenInclude(z => z.User)
                                .Include(_ => _.Polls)
                                .ThenInclude(_ => _.Choices)
                                .ThenInclude(_ => _.Answers)
                                .Include(_ => _.Invites)
                                .Include(_ => _.CoverImage)
                                .Include(_ => _.CreatedBy)
                                .Include(_ => _.Participants)
                                .Include(_ => _.ProposedDates)
                                .FirstAsync(o => o.Invites.Any(o => o.InviteCode == inviteCode));

                var vm = _mapper.Map<EventVM>(evt);
                return vm;
            }
            catch (Exception ex)
            { throw; }
        }

        [AllowAnonymous]
        [HttpPost("AnonInvited")]
        public async Task<EventVM> AnonInvited([FromBody] string inviteCode)
        {
            try
            {
                var evt = await _db.Events
                                .Include(_ => _.Participants)
                                .ThenInclude(z => z.User)
                                .Include(_ => _.Polls)
                                .ThenInclude(_ => _.Choices)
                                .ThenInclude(_ => _.Answers)
                                .Include(_ => _.Invites)
                                .Include(_ => _.CoverImage)
                                .Include(_ => _.CreatedBy)
                                .Include(_ => _.Participants)
                                .Include(_ => _.ProposedDates)
                                .FirstAsync(o => o.Invites.Any(o => o.InviteCode == inviteCode));

                var vm = _mapper.Map<EventVM>(evt);
                return vm;
            }
            catch (Exception ex)
            { throw; }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var evt = await _db.Events
                                    .Include(o => o.Participants)
                                    .Where(o => o.Id == id && o.CreatedById == currentUserId).FirstAsync();
                _db.Participation.RemoveRange(evt.Participants);
                _db.Events.Remove(evt);
                var result = await _db.SaveChangesAsync();
                await _hub.EventDeleted(id);

                if (result > 0)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GenerateInvite()
        {
            // use MlkPwgen
            // PasswordGenerator.Generate(length: 10, allowed: Sets.Alphanumerics);
            var newInvite = Guid.NewGuid().ToString().Substring(0, 8);
            while (_db.Invites.Any(o => o.InviteCode == newInvite))
            {
                newInvite = Guid.NewGuid().ToString().Substring(0, 8);
            }
            return newInvite;
        }
    }
}
