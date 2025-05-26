using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class VoucherRepository(Context context) : BaseRepository<Voucher>(context), IVoucherRepository
    {
        private readonly Context _context = context;

        public async Task<IEnumerable<Voucher>> GetVoucherListAsync(bool tracking = false)
        {
            if (tracking)
            {
                return await _context.Vouchers
                    .ToListAsync();
            }

            return await _context.Vouchers
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
