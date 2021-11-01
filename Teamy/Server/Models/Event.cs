using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime When { get; set; }
        public string? Where { get; set; }
        public List<Poll> Polls { get; set; }
        public List<Participation> Participants { get; set; }
        public List<Invite> Invites { get; set; }
        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public Guid CoverImageId { get; set; }
        public ImageModel CoverImage { get; set; }
    }

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.CreatedById);
            builder.Property(o => o.Title).HasMaxLength(128);
            builder.Property(o => o.Description).HasMaxLength(1024);
            builder.Property(o => o.Where).HasMaxLength(256);

            builder.HasMany(o => o.Invites).WithOne(o => o.Event).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.Polls).WithOne(o => o.Event).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.Participants).WithOne(o => o.Event).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

