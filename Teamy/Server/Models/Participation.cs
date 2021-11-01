using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teamy.Shared.Common;

namespace Teamy.Server.Models
{
    public class Participation
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        //public AppUser User { get; set; }
        public Guid InviteId { get; set; }
        //public Invite Invite { get; set; }
        public ParticipationStatus Status { get; set; }
    }

    public class ParticipationConfiguration : IEntityTypeConfiguration<Participation>
    {
        public void Configure(EntityTypeBuilder<Participation> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.UserId);
        }
    }
}
