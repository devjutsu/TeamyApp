using Teamy.Shared.Common;

namespace Teamy.Server.Models.Quizes
{
    public class Quiz : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ImageModel? Image { get; set; }
        public string Details { get; set; }
        public virtual List<QuizQuestion> Questions { get; set; }
        public virtual List<QuizCompletion> Completions { get; set; }
        public string CreatorId {get; set;}
        public virtual AppUser Creator { get; set; }
    }

    public class QuizQuestion : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }
        public QuizElementType Type { get; set; }
        public virtual List<QuizChoice> Choices { get; set; }
        public virtual List<QuizAnswer> Answers { get; set; }
    }

    public class QuizChoice : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid QuizQuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public string Choice { get; set; }
    }

    public class QuizAnswer : BaseEntity
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public string Answer { get; set; }
    }

    public class QuizCompletion : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; } // index
        public virtual Quiz Quiz { get; set; }
        public QuizCompletionStatus Status { get; set; }
        public string? UserId { get; set; } // index
    }

    public enum QuizCompletionStatus
    {
        Entered = 0,
        Submitted = 1
    }
}
