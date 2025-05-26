using DTOs;

namespace Service.Contracts
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherDTO>> GetVoucherListAsync();
    }
}
