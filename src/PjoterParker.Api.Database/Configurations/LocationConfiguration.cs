using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Api.Database.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(x => x.City).IsRequired().HasMaxLength(255);
            builder.Property(b => b.Street).IsRequired().HasMaxLength(255);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(255);
        }
    }
}
