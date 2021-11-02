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
        //private readonly IMapper _mapper;
        public EventsController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    UserManager<AppUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        [HttpGet("Upcoming/{count}")]
        public List<EventVM> Upcoming(int count = 9)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var events = _db.Events
                            .Include(_ => _.Participants)
                            .Include(_ => _.Polls)
                            .Include(_ => _.Invites)
                            .Include(_ => _.CoverImage)
                            .Where(_ => _.CreatedById == currentUserId || _.Participants.Any(p => p.UserId == currentUserId))
                            .OrderBy(_ => _.When)
                            .Take(count)
                            .ToList();

            return events.Select(e => e.ToVM()).ToList();
        }

        [HttpGet("Get/{id}")]
        public async Task<EventVM> Get(Guid id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var evt = await _db.Events
                            .Include(_ => _.Participants)
                            .Include(_ => _.Polls)
                            .Include(_ => _.Invites)
                            .Include(_ => _.CoverImage)
                            .Where(_ => _.CreatedById == currentUserId || _.Participants.Any(p => p.UserId == currentUserId))
                            .FirstAsync(o => o.Id == id);

            return evt.ToVM();
        }
    }

    public static class SomeExtensions
    {
        public static EventVM ToVM(this Event evt)
            => new EventVM()
            {
                CreatedById = evt.CreatedById,
                Description = evt.Description,
                Id = evt.Id,
                When = evt.When,
                Title = evt.Title,
                ImageUrl = evt.CoverImage.Url
            };
    }
}
