using DTOs;

namespace Service.Contracts
{
    public interface IShippingService
    {
        Task<IEnumerable<ShippingDTO>> GetShippingListAsync();
    }
}
