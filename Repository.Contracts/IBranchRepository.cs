using Entities;

namespace Repository.Contracts
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {
        Task<IEnumerable<Branch>> GetBranchListAsync(bool tracking = false);
    }
}
