using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Logic;
using Teamy.Server.Models;
using Teamy.Server.Services;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlternativesController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        IManageEvents _evt;
        public AlternativesController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    IMapper mapper,
                                    IVoteHub hub,
                                    IManageEvents eventLogic)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _hub = hub;
            _evt = eventLogic;
        }

        [HttpPost("RecommendDate")]
        public async Task<IActionResult> RecommendDate([FromBody] ProposedDateVM dateVM)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (dateVM.CreatedById != currentUserId)
                return BadRequest("User doesn't match");
            else if (dateVM.EventId == Guid.Empty)
                return BadRequest("Bad event id");

            var newDate = _mapper.Map<ProposedDateVM, ProposedDate>(dateVM);

            var entity = await _db.ProposedDates.AddAsync(newDate);
            await _db.SaveChangesAsync();
            await _hub.EventUpdated(entity.Entity.EventId);
            return Ok(entity.Entity);
        }

        [HttpPost("UpdateRecommendedDate")]
        public async Task<IActionResult> UpdateRecommendedDate([FromBody] ProposedDateVM dateVM)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (dateVM.CreatedById != currentUserId)
                return BadRequest("User doesn't match");
            else if (dateVM.EventId == Guid.Empty)
                return BadRequest("Bad event id");

            var newDate = _mapper.Map<ProposedDateVM, ProposedDate>(dateVM);

            var result = await _evt.UpdateRecommendedDate(newDate);
            if (result != null)
            {
                var responseDate = _mapper.Map<ProposedDate, ProposedDateVM>(result);
                return Ok(responseDate);
            }
            else return BadRequest("No such date");
        }

        [HttpPost("DeleteRecommendedDate")]
        public async Task<IActionResult> DeleteRecommendedDate([FromBody] ProposedDateVM dateVM)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (dateVM.CreatedById != currentUserId)
                return BadRequest("User doesn't match");
            else if (dateVM.EventId == Guid.Empty)
                return BadRequest("Bad event id");


            if (await _evt.DeleteRecommendedDate(dateVM.Id, dateVM.CreatedById))
                return Ok(true);
            else
                return BadRequest("No such date");
        }
    }
}
