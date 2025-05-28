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

        public async Task<Payment?> GetPaymentByIdAsync(string id, bool tracking = false)
        {
            IQueryable<Payment> query = tracking ? _context.Payments : _context.Payments.AsNoTracking();

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
