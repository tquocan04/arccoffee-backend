using Entities;
using Entities.Context;
using Repository.Contracts;

namespace Repositories
{
    public class BillRepository(Context context) : BaseRepository<Order>(context), IBillRepository
    {
        private readonly Context _context = context;
    }
}
