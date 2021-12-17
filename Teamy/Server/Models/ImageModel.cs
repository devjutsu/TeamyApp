using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class ImageModel: BaseEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }

    public class ImageModelConfiguration : IEntityTypeConfiguration<ImageModel>
    {
        public void Configure(EntityTypeBuilder<ImageModel> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.Id);
            builder.Property(o => o.Url).HasMaxLength(512);
        }
    }
}
