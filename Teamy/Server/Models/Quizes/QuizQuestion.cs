using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teamy.Shared.Common;

namespace Teamy.Server.Models.Quizes
{
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

    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.HasIndex(o => o.Id);
        }
    }
}
