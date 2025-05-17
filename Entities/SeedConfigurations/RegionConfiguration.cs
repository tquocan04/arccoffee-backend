using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedConfigurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasData(
                    new Region { Id = "Bac", Name = "Miền Bắc" },
                    new Region { Id = "Trung", Name = "Miền Trung" },
                    new Region { Id = "Nam", Name = "Miền Nam" }
                );
        }
    }
}
 