using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public string CreatorId { get; set; }
        public virtual AppUser Creator { get; set; }
        public virtual List<QCode> QCodes { get; set; }
    }

    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasIndex(o => o.Id);
        }
    }
}
