using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class PollChoice
    {
        public Guid Id { get; set; }
        public Guid VoteOptionId { get; set; }
        public Poll Poll { get; set; }
        public string Choice { get; set; }
        public List<PollAnswer> Answers { get; set; }
    }
    public class PollChoiceConfiguration : IEntityTypeConfiguration<PollChoice>
    {
        public void Configure(EntityTypeBuilder<PollChoice> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.VoteOptionId);
            builder.HasMany(o => o.Answers).WithOne(o => o.PollOption).OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.Choice).HasMaxLength(256);
        }
    }
}
