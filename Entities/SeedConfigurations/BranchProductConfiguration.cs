using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedConfigurations
{
    public class BranchProductConfiguration : IEntityTypeConfiguration<BranchProduct>
    {
        public void Configure(EntityTypeBuilder<BranchProduct> builder)
        {
            builder.HasKey(bp => new { bp.ProductId, bp.BranchId });
        }
    }
}
