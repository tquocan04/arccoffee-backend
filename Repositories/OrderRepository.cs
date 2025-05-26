using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class OrderRepository(Context context) : BaseRepository<Order>(context), IOrderRepository
    {
        private readonly Context _context = context;

        public async Task<Order> GetCartByCustomerIdAsync(string customerId, bool tracking = false)
        {
            if (!tracking)
            {
                return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstAsync(o => o.UserId == customerId
                              && o.IsCart == true);
            }

            return await _context.Orders
                .Include(o => o.Items)
                .FirstAsync(o => o.UserId == customerId
                              && o.IsCart == true);
        }
        
        public async Task<Order> GetCartWithoutItemsByCustomerIdAsync(string customerId, bool tracking = false)
        {
            if (!tracking)
            {
                return await _context.Orders
                .AsNoTracking()
                .FirstAsync(o => o.UserId == customerId
                              && o.IsCart);
            }

            return await _context.Orders
                .FirstAsync(o => o.UserId == customerId
                              && o.IsCart);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id, bool tracking = false)
        {
            if (!tracking)
            {
                return await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == id);
            }

            return await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
