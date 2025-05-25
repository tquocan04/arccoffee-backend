using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class OrderRepository(Context context) : BaseRepository<Order>(context), IOrderRepository
    {
        private readonly Context _context = context;

        public async Task<Order> GetCartByCustomerIdAsync(string customerId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstAsync(o => o.UserId == customerId
                              && o.IsCart == true);
        }
    }
}
