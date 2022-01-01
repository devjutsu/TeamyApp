using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teamy.Shared.Common;

namespace Teamy.Server.Models.Quizes
{
    public class QuizCompletion : BaseEntity
    {
        public Guid Id { get; set; }
        public QuizCompletionStatus Status { get; set; }
        public string? UserId { get; set; } // index
        public string QCodeId { get; set; }
        public virtual QCode QCode { get; set; }
    }

    public class QuizCompletionConfiguration : IEntityTypeConfiguration<QuizCompletion>
    {
        public void Configure(EntityTypeBuilder<QuizCompletion> builder)
        {
            builder.HasIndex(o => o.Id);

            builder.HasIndex(o => o.UserId);

            builder.HasOne(y => y.QCode)
                .WithMany(x => x.Completions)
                .HasForeignKey(x => x.QCodeId);
        }
    }
}
