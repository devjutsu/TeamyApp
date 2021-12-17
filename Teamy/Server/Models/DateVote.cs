using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class DateVote : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public Guid ProposedDateId { get; set; }
        public ProposedDate ProposedDate { get; set; }
    }

    public class DateVoteConfiguration : IEntityTypeConfiguration<DateVote>
    {
        public void Configure(EntityTypeBuilder<DateVote> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.UserId);
            builder.HasOne(o => o.User).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
