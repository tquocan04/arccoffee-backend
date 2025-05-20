using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class RegionRepository(Context context) : IRegionRepository
    {
        private readonly Context _context = context;

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await _context.Regions
                .AsNoTracking()
                .Include(r => r.Cities)
                .ThenInclude(c => c.Districts)
                .ToListAsync();
        }
    }
}
