using DTOs.Requests;
using DTOs;

namespace Service.Contracts
{
    public interface IBranchService
    {
        Task<BranchDTO> CreateNewBranchAsync(CreateBranchRequest req);
    }
}
