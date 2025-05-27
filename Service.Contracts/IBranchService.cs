using DTOs.Requests;
using DTOs;

namespace Service.Contracts
{
    public interface IBranchService
    {
        Task<BranchDTO> CreateNewBranchAsync(CreateBranchRequest req);
        Task<IList<BranchDTO>> GetBranchListAsync();
        Task<BranchDTO> GetBranchByIdAsync(Guid id);
        Task DeleteBranchAsync(Guid id);
    }
}
