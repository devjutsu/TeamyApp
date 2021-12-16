using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models.Templates
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Where { get; set; }
        public List<TemplatePoll> Polls { get; set; }
        //public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public TemplateCategory Category { get; set; }
        public Guid CoverImageId { get; set; }
        public ImageModel CoverImage { get; set; }
    }

    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.CategoryId);
            builder.Property(o => o.Title).HasMaxLength(128);
            builder.Property(o => o.Description).HasMaxLength(1024);
            builder.Property(o => o.Where).HasMaxLength(256);
            builder.HasMany(o => o.Polls).WithOne(o => o.Template).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
