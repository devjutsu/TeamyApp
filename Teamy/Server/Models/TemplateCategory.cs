using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class TemplateCategory
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<Template> Templates { get; set; }
        //public string ImageUrl { get; set; }
    }

    public class TemplateCategoryConfiguration : IEntityTypeConfiguration<TemplateCategory>
    {
        public void Configure(EntityTypeBuilder<TemplateCategory> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Title).HasMaxLength(128);
            builder.HasMany(o => o.Templates).WithOne(o => o.Category).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
