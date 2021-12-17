using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Teamy.Server.Data;
using Teamy.Server.Models;
using Teamy.Server.Models.Quizes;
using Teamy.Shared.ViewModels;
using Teamy.Shared.Common;

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

        [HttpGet("List")]
        public async Task<List<QuizVM>> Quizes()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var quizes = await _db.Quiz
                                    .Include(_ => _.Questions)
                                    .ThenInclude(_ => _.Choices)
                                    .Include(_ => _.Image)
                                    .Where(_ => _.CreatorId == currentUserId)
                                    .ToListAsync();

            var vms = _mapper.Map<List<Quiz>, List<QuizVM>>(quizes);
            return vms;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(QuizVM quiz)
        {
            var newQuiz = _mapper.Map<QuizVM, Quiz>(quiz);

            await _db.Quiz.AddAsync(newQuiz);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Update")]
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
        public async Task<QuizVM> Get(QuizCodeVM request)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == null)
            {
                if (string.IsNullOrEmpty(request.UserId))
                {
                    var user = await _db.Users.AddAsync(new AppUser()
                    {
                        DisplayName = "anonymous"
                    });
                    currentUserId = user.Entity.Id;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    currentUserId = request.UserId;
                }
            }

            var codeQuiz = await _db.QCodes.FindAsync(request.QCode);
            var quiz = await _db.Quiz
                                    .Include(_ => _.Questions)
                                    .ThenInclude(_ => _.Choices)
                                    .Include(_ => _.Questions)
                                    .ThenInclude(_ => _.Answers.Where(z => z.UserId == currentUserId))
                                    .Include(_ => _.Image)
                                    .FirstAsync(o => o.Id == codeQuiz.QuizId);

            var completion = await _db.QuizCompletions.Where(_ => _.QuizId == codeQuiz.QuizId
                                && _.UserId == currentUserId).FirstOrDefaultAsync();
            if (completion == null)
            {
                await _db.QuizCompletions.AddAsync(new QuizCompletion()
                {
                    Status = QuizCompletionStatus.Entered,
                    UserId = currentUserId,
                    QuizId = quiz.Id
                });
            }

            var vm = _mapper.Map<Quiz, QuizVM>(quiz);
            vm.UserId = currentUserId;
            return vm;
        }

        [AllowAnonymous]
        [HttpPost("PostAnswer")]
        public async Task<IActionResult> PostAnswer(QuizAnswerVM answer)
        {
            var answerModel = _mapper.Map<QuizAnswerVM, QuizAnswer>(answer);

            var question = await _db.QuizQuestions.FindAsync(answer.QuizQuestionId);

            switch (question.Type)
            {
                case QuizElementType.MultiSelectQuestion:
                    var myAnswerToDelete = await _db.QuizAnswers
                                                .Where(o => o.Id == answer.Id && o.UserId == answer.UserId)
                                                .FirstOrDefaultAsync();
                    if (myAnswerToDelete != null)
                        _db.QuizAnswers.Remove(myAnswerToDelete);
                    else
                        _db.QuizAnswers.Add(answerModel);
                    break;

                case QuizElementType.SingleSelectQuestion:
                case QuizElementType.GradeQuestion:
                case QuizElementType.FreeTextQuestion:
                    var myAnswers = await _db.QuizAnswers
                                            .Include(_ => _.Answer)
                                            .Where(_ => _.QuizQuestionId == answer.QuizQuestionId && _.UserId == answer.UserId)
                                            .ToListAsync();
                    _db.QuizAnswers.RemoveRange(myAnswers);
                    _db.QuizAnswers.Add(answerModel);
                    break;

                default:
                    // no action
                    break;
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
