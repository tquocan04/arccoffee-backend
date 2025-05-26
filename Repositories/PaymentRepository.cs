using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class PaymentRepository(Context context) : BaseRepository<Payment>(context), IPaymentRepository
    {
        private readonly Context _context = context;

        public async Task<IEnumerable<Payment>> GetPaymentListAsync(bool tracking = false)
        {
            if (tracking)
            {
                return await _context.Payments
                    .ToListAsync();
            }

            return await _context.Payments
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
