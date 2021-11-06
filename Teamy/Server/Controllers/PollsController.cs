using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
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
        private readonly IMapper _mapper;
        public PollsController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    UserManager<AppUser> userManager,
                                    IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("Vote")]
        public async Task<IActionResult> Vote([FromBody] PollChoiceVM choice)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;

            var poll = await _db.Polls.FindAsync(choice.PollId);
            var answer = new PollAnswer() { PollChoiceId = choice.Id.Value, UserId = currentUserId };
            if(poll.MultiChoice)
            {
                _db.PollAnswers.Add(answer);
            }
            else
            {
                var myAnswers = await _db.PollAnswers.Where(o => o.UserId == currentUserId
                                                        && o.PollChoiceId == choice.PollId)
                                                .ToListAsync();
                _db.PollAnswers.RemoveRange(myAnswers);
                _db.PollAnswers.Add(answer);
            }
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}