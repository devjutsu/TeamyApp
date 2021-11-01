﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teamy.Server.Models
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime When { get; set; }
        public string Where { get; set; }
        public List<TemplatePoll> Polls { get; set; }
        //public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public TemplateCategory Category { get; set; }
    }

    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.CategoryId);
        }
    }
}
