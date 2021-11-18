﻿using AutoMapper;
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
    public class TemplatesController : ControllerBase
    {
        ILogger<EventsController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        public TemplatesController(ILogger<EventsController> logger,
                                    TeamyDbContext db,
                                    IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<List<EventVM>> Get()
        {
            var templates = await _db.Templates
                                .Include(_ => _.Polls)
                                .ThenInclude(p => p.Choices)
                                .Include(_ => _.CoverImage)
                                .ToListAsync();

            //var vms = _mapper.Map<List<Template>, List<EventVM>>(tpls).ToList();
            var events = templates.Select(_ => _mapper.Map<Event>(_)).ToList();
            var vms = _mapper.Map<List<Event>, List<EventVM>>(events);
            vms.ForEach(v => 
            {
                v.Id = null;
                v.When = DateTime.Today.AddDays(1).AddHours(12);
                v.ProposedDates = new List<ProposedDateVM>() { new ProposedDateVM() { Date = DateTime.Today.AddDays(1) } };
                v.Polls?.ForEach(p =>
                {
                    p.Id = null;
                    p.Choices.ForEach(c => c.Id = null);
                });
            });
            // @! only this weird mapping works: List<TemplatePollChoice> => List<EventPollChoiceVM> !@
            return vms;
        }
    }
}
