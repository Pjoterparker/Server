using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Api.Database.Configurations
{
    public class UniquenessTableConfiguration : IEntityTypeConfiguration<UniquenessTable>
    {
        public void Configure(EntityTypeBuilder<UniquenessTable> builder)
        {
            builder.Property(x => x.Key).IsRequired().HasMaxLength(1024);
            builder.Property(x => x.Value).IsRequired().HasMaxLength(1024);

            builder.HasKey(x => new { x.Key, x.Value, x.AggrageteId });
        }
    }
}