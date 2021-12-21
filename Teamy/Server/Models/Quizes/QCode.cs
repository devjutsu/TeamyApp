using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Quizes
{
    public class QCode : BaseEntity
    {
        public string Id { get; set; }
        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public virtual List<QuizCompletion> Completions { get; set; } = new List<QuizCompletion>();
    }

    public class QCodeConfiguration : IEntityTypeConfiguration<QCode>
    {
        public void Configure(EntityTypeBuilder<QCode> builder)
        {
            builder.HasIndex(o => o.Id);
            //builder.HasMany(o => o.Completions).WithOne().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
