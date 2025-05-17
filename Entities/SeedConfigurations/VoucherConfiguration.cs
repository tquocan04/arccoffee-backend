using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.SeedConfigurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasIndex(v => v.Code).IsUnique();

            builder.HasMany(v => v.Orders)
                    .WithOne(o => o.Voucher)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
