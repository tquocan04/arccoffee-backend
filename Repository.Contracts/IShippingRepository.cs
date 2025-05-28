using Entities;

namespace Repository.Contracts
{
    public interface IShippingRepository : IBaseRepository<ShippingMethod>
    {
        Task<IEnumerable<ShippingMethod>> GetShippingListAsync();
        Task<ShippingMethod?> GetShippingByIdAsync(string id);
    }
}
