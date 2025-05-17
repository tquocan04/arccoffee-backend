using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedConfigurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasMany(b => b.Users)
                    .WithOne(e => e.Branch)
                    .OnDelete(DeleteBehavior.SetNull);
            
            builder.HasMany(b => b.Addresses)
                    .WithOne(a => a.Branch)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
