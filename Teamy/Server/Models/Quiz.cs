using Teamy.Shared.Common;

namespace Teamy.Server.Models
{
    public class QCode
    {
        public string Id { get; set; }
        public Guid QuizId { get; set; }
    }

    public class Quiz
    {
        public Guid Id { get; set; }
        public virtual List<QuizQuestion> Questions { get; set; }
        public virtual List<QuizCompletion> Completions { get; set; }
    }

    public class QuizQuestion
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public string Question { get; set; }
        public virtual List<QuizChoice> Choices { get; set;}
        public virtual List<QuizAnswer> Answers { get; set; }
        
    }

    public class QuizChoice
    {
        public Guid Id { get; set; }
        public QuizQuestionType Type { get; set; }
        public Guid QuizQuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
    }

    public class QuizAnswer
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public string Answer { get; set; }
    }

    public class QuizCompletion
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public short Status { get; set; }
        public string? UserId { get; set; }
    }
}
