using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Api.Database.Configurations
{
    public class SpotConfiguration : IEntityTypeConfiguration<Spot>
    {
        public void Configure(EntityTypeBuilder<Spot> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(255);
        }
    }
}
