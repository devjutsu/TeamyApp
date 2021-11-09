using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teamy.Shared.Common;

namespace Teamy.Server.Models
{
    public class AnonParticipation
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public Guid InviteId { get; set; }
        public string? UnregisteredName { get; set; }
        public ParticipationStatus Status { get; set; }
    }

    public class AnonParticipationConfiguration : IEntityTypeConfiguration<AnonParticipation>
    {
        public void Configure(EntityTypeBuilder<AnonParticipation> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}
