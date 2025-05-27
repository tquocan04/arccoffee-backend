using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class BranchRepository(Context context) : BaseRepository<Branch>(context), IBranchRepository
    {
        private readonly Context _context = context;

        public async Task<IEnumerable<Branch>> GetBranchListAsync(bool tracking = false)
        {
            var query = tracking ? _context.Branches : _context.Branches.AsNoTracking();

            return await query.ToListAsync();
        }
    }
}
