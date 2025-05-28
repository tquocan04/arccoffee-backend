using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class BillRepository(Context context) : BaseRepository<Order>(context), IBillRepository
    {
        private readonly Context _context = context;

        public async Task<IEnumerable<Order>> GetBillListAsync(string? customerId, string status, bool tracking = false)
        {
            IQueryable<Order> query = tracking ?
                _context.Orders.Where(o => o.Status == status).Include(o => o.Items)
                : _context.Orders.AsNoTracking().Where(o => o.Status == status).Include(o => o.Items);

            // admin get list
            if (customerId == null)
            {
                return await query.ToListAsync();
            }

            return await query.Where(o => o.UserId == customerId).ToListAsync();
        }

        public async Task<Order?> GetBillByIdAsync(Guid id, bool tracking = false)
        {
            IQueryable<Order> query = tracking ?
                _context.Orders.Include(o => o.Items)
                : _context.Orders.AsNoTracking().Include(o => o.Items);

            return await query.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetCompletedBillByYearMonthAsync(int year, int? month = null)
        {
            IQueryable<Order> query = _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Items)
                    .Where(o => o.Status == "Completed" && o.OrderDate.Year == year);

            if (month != null)
            {
                return await query
                    .Where(o => o.OrderDate.Month == month)
                    .ToListAsync();
            }

            return await _context.Orders.ToListAsync();
        }
    }
}
