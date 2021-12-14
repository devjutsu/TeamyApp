using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Models.Quiz;
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

        [HttpPost("Get")]
        public async Task<IActionResult> Create(QuizVM quiz)
        {
            var newQuiz = _mapper.Map<QuizVM, Quiz>(quiz);

            await _db.Quiz.AddAsync(newQuiz);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Update(QuizVM quiz)
        {
            var existingQuiz = await _db.Quiz.FindAsync(quiz.Id);
            if (existingQuiz == null)
                return BadRequest("Quiz not found");

            var updatedQuiz = _mapper.Map<QuizVM, Quiz>(quiz);
            // @! update here
            existingQuiz.Questions = updatedQuiz.Questions;

            _db.Quiz.Update(existingQuiz);
            await _db.SaveChangesAsync();
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("Get")]
        public async Task<QuizVM> Get([FromBody] string qCode)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(currentUserId == null)
            {
                // @! Create anonymous user
                // and return Id with VM
                // to save in cookies or local storage
            }

            var codeQuiz = await _db.QCodes.FirstAsync();
            var quiz = await _db.Quiz
                                    .Include(_ => _.Questions)
                                    .ThenInclude(_ => _.Choices)
                                    .FirstAsync(o => o.Id == codeQuiz.QuizId);
            var vm = _mapper.Map<Quiz, QuizVM>(quiz);

            return vm;
        }

        [AllowAnonymous]
        [HttpPost("PostAnswer")]
        public async Task<IActionResult> Post(QuizAnswerVM answer)
        {
            var model = _mapper.Map<QuizAnswerVM, QuizAnswer>(answer);
            _db.QuizAnswers.Add(model);

            var codeQuiz = await _db.QCodes.FindAsync(answer.QCode);
            if (codeQuiz == null)
                return BadRequest();

            var completion = await _db.QuizCompletions.Where(_ => _.QuizId == codeQuiz.QuizId
                                && _.UserId == answer.UserId).FirstAsync();
            if(completion.Status == QuizCompletionStatus.Entered)
            {
                completion.Status = QuizCompletionStatus.Answered;
                _db.QuizCompletions.Update(completion);
            }    
            
            await _db.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Submit/{qCode}")]
        public async Task<IActionResult> Submit([FromQuery] string qCode, string userId)
        {
            var codeQuiz = await _db.QCodes.FindAsync(qCode);
            if (codeQuiz == null)
                return BadRequest();

            var completion = await _db.QuizCompletions.Where(_ => _.QuizId == codeQuiz.QuizId
                                && _.UserId == userId).FirstAsync();

            completion.Status = QuizCompletionStatus.Submitted;
            _db.QuizCompletions.Update(completion);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
