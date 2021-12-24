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

        [Authorize]
        [HttpGet("ManageList")]
        public async Task<List<QuizVM>> ManageList()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var quizes = await _db.Quiz
                                    .Include(_ => _.QCodes)
                                    .ThenInclude(_ => _.Completions)
                                    .Where(_ => _.CreatorId == currentUserId)
                                    .ToListAsync();

            var vms = _mapper.Map<List<Quiz>, List<QuizVM>>(quizes);
            return vms;
        }

        [Authorize]
        [HttpPost("GenerateQCode")]
        public async Task<QuizVM> GenerateQCode(QuizIdVM vm)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var quiz = await _db.Quiz
                                    .Include(_ => _.QCodes)
                                    .ThenInclude(_ => _.Completions)
                                    .Where(_ => _.CreatorId == currentUserId && _.Id == vm.Id)
                                    .FirstAsync();

            quiz.QCodes.Add(new QCode()
            {
                Id = GenQCode(),
                QuizId = quiz.Id
            });
            _db.Update(quiz);
            await _db.SaveChangesAsync();

            var resultVM = _mapper.Map<Quiz, QuizVM>(quiz);
            return resultVM;
        }

        [Authorize]
        [HttpPost("UpdateQCodeInfo")]
        public async Task<IActionResult> UpdateQCodeInfo(QuizCodeVM qcodevm)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var qcode = await _db.QCodes
                                .Include(_ => _.Quiz).Where(o => o.Id == qcodevm.Id && o.Quiz.CreatorId == currentUserId).FirstOrDefaultAsync();

            if(qcode == null)
                return BadRequest("No qcode for this user");

            qcode.Url = qcodevm.Url;
            qcode.Comment = qcodevm.Comment;
            _db.Update(qcode);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost("GetAnswers")]
        public async Task<List<QuizQuestionVM>> GetAnswers(QCodeIdRequest vm)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var qcode = await _db.QCodes
                        .Include(_ => _.Quiz)
                        .Include(_ => _.Completions)
                        .Where(_ => _.Id == vm.Id && _.Quiz.CreatorId == currentUserId)
                        .FirstAsync();
            
            var userIds = qcode.Completions.Select(c => c.UserId).ToList();
            var questions = _db.QuizQuestions
                            .Include(_ => _.Answers.Where(a => userIds.Contains(a.UserId)))
                            .Where(_ => _.QuizId == qcode.QuizId).ToList();

            var questionVMs = _mapper.Map<List<QuizQuestion>, List<QuizQuestionVM>>(questions);

            return questionVMs;
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
                if (!string.IsNullOrEmpty(request.UserId) && await _db.Users.FindAsync(request.UserId) != null)
                {
                    currentUserId = request.UserId;
                }
                else
                {
                    var user = await _db.Users.AddAsync(new AppUser()
                    {
                        DisplayName = "anonymous"
                    });
                    currentUserId = user.Entity.Id;
                    await _db.SaveChangesAsync();
                }
            }

            var codeQuiz = await _db.QCodes.FindAsync(request.Id);
            var quiz = await _db.Quiz
                                    .Include(_ => _.Questions)
                                    .ThenInclude(_ => _.Choices)
                                    .Include(_ => _.Questions)
                                    .ThenInclude(_ => _.Answers.Where(z => z.UserId == currentUserId))
                                    .Include(_ => _.Image)
                                    .FirstOrDefaultAsync(o => o.Id == codeQuiz.QuizId);

            var completion = await _db.QuizCompletions.Include(_ => _.QCode)
                                    .Where(_ => _.QCode.QuizId == codeQuiz.QuizId
                                && _.UserId == currentUserId).FirstOrDefaultAsync();
            if (completion == null)
            {
                await _db.QuizCompletions.AddAsync(new QuizCompletion()
                {
                    Status = QuizCompletionStatus.Entered,
                    UserId = currentUserId,
                    QCodeId = request.Id
                });
                await _db.SaveChangesAsync();
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
                                                .Where(_ => _.QuizQuestionId == answer.QuizQuestionId && _.UserId == answer.UserId && _.Answer == answer.Answer)
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
        [HttpPost("Submit")]
        public async Task<IActionResult> Submit(QuizCodeVM request)
        {
            var codeQuiz = await _db.QCodes.FindAsync(request.Id);
            if (codeQuiz == null)
                return BadRequest();

            var completion = await _db.QuizCompletions
                                        .Include(_ => _.QCode)
                                        .Where(_ => _.QCode.QuizId == codeQuiz.QuizId
                                && _.UserId == request.UserId).FirstOrDefaultAsync();

            if (completion != null)
            {
                completion.Status = QuizCompletionStatus.Submitted;
                _db.QuizCompletions.Update(completion);
            }
            else
            {
                completion = new QuizCompletion()
                {
                    UserId = request.UserId,
                    Status = QuizCompletionStatus.Submitted,
                    QCodeId = request.Id,
                };
                _db.QuizCompletions.Add(completion);
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        private string GenQCode()
        {
            // use MlkPwgen
            // PasswordGenerator.Generate(length: 10, allowed: Sets.Alphanumerics);
            var newQcode = Guid.NewGuid().ToString().Substring(0, 8);
            while (_db.QCodes.Any(o => o.Id == newQcode))
            {
                newQcode = Guid.NewGuid().ToString().Substring(0, 8);
            }
            return newQcode;
        }
    }
}
