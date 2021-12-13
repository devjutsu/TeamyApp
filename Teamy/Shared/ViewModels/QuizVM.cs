using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.ViewModels
{
    public class QuizVM
    {
        public List<QuestionVM> Questions { get; set; }
        public string? UserId { get; set; }
        public string QCode { get; set; }
    }

    public class QuestionVM
    {
        public string Question { get; set; }
    }

    public enum QuestionType
    {
        SingleSelect,
        MultiSelect,
        Grade,
        FreeText,
    }

    public class AnswerVM
    {
        public string Answer { get; set; }
    }
}
