using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class TemplatePollChoice
    {
        public Guid Id { get; set; }
        public Guid TemplatePollId { get; set; }
        public TemplatePoll Poll { get; set; }
        public string Choice { get; set; }
    }

    public class TemplatePollChoiceConfiguration : IEntityTypeConfiguration<TemplatePollChoice>
    {
        public void Configure(EntityTypeBuilder<TemplatePollChoice> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.TemplatePollId);
            builder.Property(o => o.Choice).HasMaxLength(256);
        }
    }
}
