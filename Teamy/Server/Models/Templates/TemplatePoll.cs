using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Templates
{
    public class TemplatePoll
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public Template Template { get; set; }
        public string Question { get; set; }
        public List<TemplatePollChoice> Choices { get; set; }
        public bool MultiChoice { get; set; }
        public bool FreeTextOption { get; set; }
    }

    public class TemplatePollConfiguration : IEntityTypeConfiguration<TemplatePoll>
    {
        public void Configure(EntityTypeBuilder<TemplatePoll> builder)
        {
            builder.HasIndex(o => o.Id);
            builder.HasIndex(o => o.TemplateId);
            builder.HasMany(o => o.Choices).WithOne(o => o.Poll).OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.Question).HasMaxLength(256);
        }
    }
}
