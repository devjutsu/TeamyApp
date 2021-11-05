using AutoMapper;
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
        //IStorageService _storage { get; set; }
        //IEventHub _hub;
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

        [HttpGet("Get/{id}")]
        public async Task<EventVM> Get(Guid id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

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
    }
}
