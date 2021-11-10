using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Services;
using Teamy.Shared.Common;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvitesController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        public InvitesController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    IVoteHub hub,
                                    IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpPost("Respond")]
        public async Task<Guid> Respond([FromBody] ParticipationVM participation)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentParticipations = _db.Participation.Where(o => o.UserId == currentUserId && o.EventId == participation.EventId);
            _db.Participation.RemoveRange(currentParticipations);
            var entity = _db.Participation.Add(new Participation() { UserId = currentUserId, EventId = participation.EventId, Status = participation.Status });

            if (participation.Status == ParticipationStatus.Reject)
            {
                var pollAnswers = _db.PollAnswers
                    .Include(_ => _.PollChoice)
                    .ThenInclude(_ => _.Poll)
                    .Where(_ => _.PollChoice.Poll.EventId == participation.EventId && _.UserId == currentUserId);

                _db.PollAnswers.RemoveRange(pollAnswers);
            }

            await _db.SaveChangesAsync();
            await _hub.EventUpdated(participation.EventId);
            return entity.Entity.Id;
        }

        [AllowAnonymous]
        [HttpPost("RespondAnon")]
        public async Task<Guid> RespondAnon([FromBody] ParticipationVM participation)
        {
            var invite = _db.Invites.First(o => o.InviteCode == participation.InviteCode);

            var answer = new AnonParticipation()
            {
                EventId = participation.EventId,
                InviteId = invite.Id,
                Status = participation.Status,
                UnregisteredName = participation.Name,
            };

            var entity = await _db.AnonParticipation.AddAsync(answer);

            await _db.SaveChangesAsync();
            await _hub.EventUpdated(participation.EventId);
            return entity.Entity.Id;
        }
    }
}
