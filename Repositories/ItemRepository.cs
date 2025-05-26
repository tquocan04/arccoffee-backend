using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class ItemRepository(Context context) : BaseRepository<Item>(context), IItemRepository
    {
        private readonly Context _context = context;



        public async Task<Item?> GetItemAsync(Guid orderId, Guid productId, bool tracking = false)
        {
            if (!tracking)
            {
                return await _context.Items
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.OrderId == orderId
                            && i.ProductId == productId);
            }

            return await _context.Items
                .FirstOrDefaultAsync(i => i.OrderId == orderId
                        && i.ProductId == productId);
        }
    }
}
