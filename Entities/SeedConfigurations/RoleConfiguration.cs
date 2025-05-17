using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "ADMIN",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new Role
                {
                    Id = "STAFF",
                    Name = "Staff",
                    NormalizedName = "STAFF",
                },
                new Role
                {
                    Id = "CUSTOMER",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                }
            );
        }
    }
}
