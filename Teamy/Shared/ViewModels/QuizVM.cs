using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teamy.Shared.Common;

namespace Teamy.Shared.ViewModels
{
    public class QuizVM
    {
        public Guid Id { get; set; }
        public List<QuestionVM> Questions { get; set; }
        public string? UserId { get; set; }
    }

    public class QuestionVM
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public string Question { get; set; }
        public QuizQuestionType Type { get; set; }
        public string Answer { get; set; }
    }

    public class QuizAnswerPostVM
    {
        public Guid QuizQuestionId { get; set; }
        public string? Answer { get; set; }
        public string QCode { get; set; }
    }
}
