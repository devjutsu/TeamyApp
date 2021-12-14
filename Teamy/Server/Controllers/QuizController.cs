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
    public class QuizController : ControllerBase
    {
        ILogger<QuizController> _logger { get; set; }
        TeamyDbContext _db { get; set; }
        private readonly IMapper _mapper;
        public QuizController(ILogger<QuizController> logger,
                                    TeamyDbContext db,
                                    IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Get")]
        public async Task<QuizVM> Get([FromBody] string qCode)
        {
            

            return new QuizVM();
        }

        [AllowAnonymous]
        [HttpPost("PostAnswer")]
        public async Task Post(QuizAnswerPostVM answer)
        {
            return;
        }

        [AllowAnonymous]
        [HttpPost("Submit/{qCode}")]
        public async Task Submit([FromQuery] string qCode, string userId)
        {
            return;
        }
    }
}
