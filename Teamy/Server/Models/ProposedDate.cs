using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public record ProposedDate
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }

    public class ProposedDateConfiguration : IEntityTypeConfiguration<ProposedDate>
    {
        public void Configure(EntityTypeBuilder<ProposedDate> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}
