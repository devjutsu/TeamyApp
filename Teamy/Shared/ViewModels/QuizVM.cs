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
        public List<QuizQuestionVM> Questions { get; set; }
        public string? UserId { get; set; }
    }

    public class QuizQuestionVM
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public int OrderNumber { get; set; }
        public QuizElementType Type { get; set; }
        public List<QuizChoiceVM> Choices { get; set; }
    }

    public class QuizChoiceVM
    {
        public Guid Id { get; set; }
        public string Choice { get; set; }
    }

    public class QuizAnswerVM
    {
        public Guid? Id { get; set; }
        public string Answer { get; set; }
        public Guid QuizQuestionId { get; set; }
        public string QCode { get; set; }
        public string UserId { get; set; }
    }

    public class QuizCodeVM
    {
        public string QCode { get; set; }
        public string? UserId { get; set; }
    }
}
