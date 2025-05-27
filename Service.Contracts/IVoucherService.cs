using DTOs;
using DTOs.Requests;

namespace Service.Contracts
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherDTO>> GetVoucherListAsync();
        Task<VoucherDTO> CreateNewVoucherAsync(CreateVoucherRequest req);
        Task<VoucherDTO> GetVoucherByCodeAsync(string code);
    }
}
