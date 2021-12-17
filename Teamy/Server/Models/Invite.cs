using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class Invite : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public Event Event { get; set; }
        public string InvitedById { get; set; }
        //public AppUser InvitedBy { get; set; }
        public bool Public { get; set; }
        public string InviteCode { get; set; }
    }

    public class InviteConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.InviteCode).IsUnique();
            builder.Property(o => o.InviteCode).HasMaxLength(8).IsFixedLength();
        }
    }
}
