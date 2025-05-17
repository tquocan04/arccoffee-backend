using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedConfigurations
{
    public class ShippingConfiguration : IEntityTypeConfiguration<ShippingMethod>
    {
        public void Configure(EntityTypeBuilder<ShippingMethod> builder)
        {
            builder.HasData(
                    new ShippingMethod
                    {
                        Id = "GRAB",
                        Name = "GRAB",
                    },
                    new ShippingMethod
                    {
                        Id = "BE",
                        Name = "BE",
                    },
                    new ShippingMethod
                    {
                        Id = "SHOPEEFOOD",
                        Name = "SHOPEE FOOD",
                    });
        }
    }
}
