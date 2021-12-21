using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Quizes
{
    public class QuizChoice : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid QuizQuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public string Choice { get; set; }
    }

    public class QuizChoiceConfiguration : IEntityTypeConfiguration<QuizChoice>
    {
        public void Configure(EntityTypeBuilder<QuizChoice> builder)
        {
            builder.HasIndex(o => o.Id);
        }
    }
}
