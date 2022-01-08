using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Teamy.Server.Data;
using Teamy.Server.Logic;
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
        IManageEvents _evt;
        public EventsController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    IManageEvents eventLogic,
                                    UserManager<AppUser> userManager,
                                    IMapper mapper,
                                    IVoteHub hub)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            _hub = hub;
            _evt = eventLogic;
        }

        [HttpGet("Upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                return BadRequest("Please, authenticate!");

            var events = await _evt.GetUpcoming(currentUserId);

            var vms = _mapper.Map<List<Event>, List<EventVM>>(events);
            return Ok(vms);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                return BadRequest("Please, authenticate!");

            var evt = await _evt.Get(id, currentUserId);

            if (evt != null)
            {
                var vm = _mapper.Map<EventVM>(evt);
                return Ok(vm);
            }
            return BadRequest("Woops...");
        }

        [HttpPost("Create")]
        public async Task<EventCreatedVM> Create([FromBody] EventVM eventVM)
        {
            //var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? eventVM.CreatedById;
            var evt = _mapper.Map<Event>(eventVM);

            var result = await _evt.CreateEvent(evt, currentUserId, eventVM.ImageUrl);

            return new EventCreatedVM()
            {
                EventId = result.Id,
                UserId = result.CreatedById,
            };
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] EventVM eventVM)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;
            var newEvent = _mapper.Map<Event>(eventVM);


            var result = await _evt.UpdateEvent(newEvent, currentUserId, eventVM.ImageUrl);

            return Ok($"{result.Id}");
        }


        [HttpGet("Invited")]
        public async Task<EventVM> Invited([FromBody] string inviteCode)
        {
            var evt = await _evt.GetInvitedEvent(inviteCode);

            var vm = _mapper.Map<EventVM>(evt);
            return vm;
        }

        [AllowAnonymous]
        [HttpPost("AnonInvited")]
        public async Task<EventVM> AnonInvited([FromBody] string inviteCode)
        {
            var evt = _evt.GetInvitedEvent(inviteCode);

            var vm = _mapper.Map<EventVM>(evt);
            return vm;
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _evt.DeleteEvent(id, currentUserId);

            if (result > 0)
                return Ok();
            else
                return BadRequest();
        }
    }
}
