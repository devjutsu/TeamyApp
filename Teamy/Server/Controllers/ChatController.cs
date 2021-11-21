﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Services;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        UserManager<AppUser> _userManager { get; set; }
        private readonly IMapper _mapper;
        IVoteHub _hub;
        public ChatController(ILogger<EventsController> logger,
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

        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] ChatMessageVM message)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var displayName = (await _userManager.FindByIdAsync(currentUserId)).DisplayName;


            var entity = _mapper.Map<ChatMessage>(message);
            entity.SentBy = currentUserId;
            entity.SentAt = DateTime.Now;

            _db.Chat.Add(entity);
            
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Get/{eventId}")]
        public async Task<List<ChatMessageVM>> Get([FromQuery] Guid eventId)
        {
            var messages = await _db.Chat.Where(o => o.EventId == eventId).ToListAsync();
            var vms = _mapper.Map<List<ChatMessageVM>>(messages);
            
            return vms;
        }
    }
}
