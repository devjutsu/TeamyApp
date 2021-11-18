using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class DateVote
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ProposedDate ProposedDate { get; set; }
    }

    public class DateVoteConfiguration : IEntityTypeConfiguration<DateVote>
    {
        public void Configure(EntityTypeBuilder<DateVote> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.UserId);
        }
    }
}
