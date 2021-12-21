using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Quizes
{
    public class QuizAnswer : BaseEntity
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public string Answer { get; set; }
    }

    public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizAnswer> builder)
        {
            builder.HasIndex(o => o.Id);
        }
    }
}
