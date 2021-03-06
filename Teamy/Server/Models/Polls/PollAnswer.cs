using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Polls
{
    public class PollAnswer : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public PollChoice PollChoice { get; set; }
        public Guid PollChoiceId { get; set; }
    }

    public class PollAnswerConfiguration : IEntityTypeConfiguration<PollAnswer>
    {
        public void Configure(EntityTypeBuilder<PollAnswer> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.PollChoiceId);
        }
    }
}
