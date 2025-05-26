using Entities;

namespace Repository.Contracts
{
    public interface IVoucherRepository : IBaseRepository<Voucher>
    {
        Task<IEnumerable<Voucher>> GetVoucherListAsync(bool tracking = false);
    }
}
