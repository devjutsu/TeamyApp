using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class Poll
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public string Question { get; set; }
        public List<PollChoice> Options { get; set; }
        public bool MultiChoice { get; set; }
        public bool FreeTextOption { get; set; }
    }

    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.EventId);
            builder.HasMany(o => o.Options).WithOne(o => o.Poll).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
