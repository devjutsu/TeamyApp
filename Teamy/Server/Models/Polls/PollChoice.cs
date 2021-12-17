using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Polls
{
    public class PollChoice : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid PollId { get; set; }
        public Poll Poll { get; set; }
        public string Choice { get; set; }
        public List<PollAnswer> Answers { get; set; }
    }
    public class PollChoiceConfiguration : IEntityTypeConfiguration<PollChoice>
    {
        public void Configure(EntityTypeBuilder<PollChoice> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.PollId);
            builder.HasMany(o => o.Answers).WithOne(o => o.PollChoice).OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.Choice).HasMaxLength(256);
        }
    }
}
