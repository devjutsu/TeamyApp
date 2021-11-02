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
            var tpls = await _db.Templates
                                .Include(_ => _.Polls)
                                .Include(_ => _.CoverImage)
                                .ToListAsync();

            var vms = tpls.Select(_ => _mapper.Map<Template, EventVM>(_)).ToList();
            
            // @! should fix mapping of List<TemplatePollChoice> => List<EventPollChoiceVM> !@
            return vms;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<EventVM> Get(Guid id)
        {
            var tpl = await _db.Templates
                                .Include(_ => _.Polls)
                                .Include(_ => _.CoverImage)
                                .Where(_ => _.Id == id)
                                .SingleAsync();

            return _mapper.Map<Template, EventVM>(tpl);
        }
    }
}
