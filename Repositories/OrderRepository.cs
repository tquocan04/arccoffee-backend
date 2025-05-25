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

        public async Task<Item?> GetItemAsync(Guid orderId, Guid productId)
        {
            return await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.OrderId == orderId
                        && i.ProductId == productId);
        }

        public void AddItemToCart(Item item)
        {
            _context.Items.Add(item);
        }

        public void UpdateQuantityItemToCart(Item item)
        {
            _context.Items.Update(item);
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
