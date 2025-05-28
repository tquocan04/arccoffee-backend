using Entities;

namespace Repository.Contracts
{
    public interface IBillRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetBillListAsync(string? customerId, string status, bool tracking = false);
    }
}
