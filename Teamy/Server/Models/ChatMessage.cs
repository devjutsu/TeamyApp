using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string PostedBy { get; set; }
        public string Text { get; set; }
        public Guid EventId { get; set; }
    }

    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.EventId);
        }
    }
}
