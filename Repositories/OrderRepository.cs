using Entities;
using Entities.Context;
using Repository.Contracts;

namespace Repositories
{
    public class OrderRepository(Context context) : BaseRepository<Order>(context), IOrderRepository
    {
        private readonly Context _context = context;
    }
}
