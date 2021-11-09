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
    public class EventsController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        UserManager<AppUser> _userManager { get; set; }
        private readonly IMapper _mapper;
        public EventsController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    UserManager<AppUser> userManager,
                                    IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EventVM eventVM)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;

            var evt = _mapper.Map<Event>(eventVM);
            evt.CreatedById = currentUserId;
            evt.CreatedBy = await _db.Users.FindAsync(currentUserId);
            evt.Invites = new List<Invite>() { new Invite() { InviteCode = GenerateInvite(), InvitedById = currentUserId, Public = true } };
            if (string.IsNullOrEmpty(eventVM.ImageUrl))
                evt.CoverImage = null;

            try
            {
                var addedEvt = _db.Events.Add(evt);
                await _db.SaveChangesAsync();
                return Ok($"{addedEvt.Entity.Id}");
            }
            catch (Exception ex)
            { throw; }
        }

        [HttpGet("Upcoming/{count}")]
        public List<EventVM> Upcoming(int count = 9)
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
                                .Where(_ => _.CreatedById == currentUserId || _.Participants.Any(p => p.UserId == currentUserId))
                                .OrderBy(_ => _.When)
                                .Take(count)
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
                                .Where(_ => _.CreatedById == currentUserId || _.Participants.Any(p => p.UserId == currentUserId))
                                .FirstAsync(o => o.Id == id);

                var vm = _mapper.Map<EventVM>(evt);
                return vm;
            }
            catch (Exception ex)
            { throw; }
        }

        [AllowAnonymous]
        [HttpGet("Invited")]
        public async Task<EventVM> Invited()
        {
            try
            {
                var inviteCode = "d8e1f7b0";
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
                                .FirstAsync(o => o.Invites.Any(o => o.InviteCode == inviteCode));

                var vm = _mapper.Map<EventVM>(evt);
                return vm;
            }
            catch (Exception ex)
            { throw; }
        }

        //[AllowAnonymous]
        //[HttpPost("Test")]
        //public ActionResult<PollAnswerVM> Test([FromBody] string test)
        //{
        //    var ans = new PollAnswerVM() { Id = Guid.NewGuid(), UserId = "123", UserName = "543" };
        //    return Ok(ans);
        //}

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
