using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class ShippingRepository : BaseRepository<ShippingMethod>, IShippingRepository
    {
        private readonly Context _context;

        public ShippingRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShippingMethod>> GetShippingListAsync()
        {
            return await _context.ShippingMethods
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
