using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid EventId { get; set; }
        public string? SentById { get; set; }
        public AppUser? SentBy { get; set; }
        public DateTime SentAt { get; set; }
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
