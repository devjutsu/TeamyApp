using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime When { get; set; }
        public string? Where { get; set; }
        //public List<InviteResponse> EventResponses { get; set; }
        //public string InviteCode { get; set; }
        public string CreatedByUserId { get; set; }
        public virtual AppUser CreatedByUser { get; set; }
        public List<Poll> Polls { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(o => o.Id);
            //builder.HasIndex(o => o.InviteCode).IsUnique();
            //builder.Property(o => o.InviteCode).HasMaxLength(8);
            builder.HasIndex(o => o.CreatedByUserId);

            //builder.HasMany(o => o.EventResponses).WithOne(o => o.Event).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.Polls).WithOne(o => o.Event).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
