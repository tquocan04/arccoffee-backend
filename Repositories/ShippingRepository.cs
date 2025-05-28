using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class ShippingRepository(Context context) : BaseRepository<ShippingMethod>(context), IShippingRepository
    {
        private readonly Context _context = context;

        public async Task<IEnumerable<ShippingMethod>> GetShippingListAsync()
        {
            return await _context.ShippingMethods
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<ShippingMethod?> GetShippingByIdAsync(string id)
        {
            return await _context.ShippingMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
