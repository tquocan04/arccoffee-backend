using DTOs;
using DTOs.Requests;
using Entities;

namespace Service.Contracts
{
    public interface IBillService
    {
        Task<BillDTO> CreateNewBillAsync(string email, BillRequest req);
        Task<IEnumerable<BillDTO>> GetBillListAsync(string? email, string status);
    }
}
