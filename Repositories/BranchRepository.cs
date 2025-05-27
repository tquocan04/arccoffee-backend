using Entities;
using Entities.Context;
using Repository.Contracts;

namespace Repositories
{
    public class BranchRepository(Context context) : BaseRepository<Branch>(context), IBranchRepository
    {
        private readonly Context _context = context;
    }
}
