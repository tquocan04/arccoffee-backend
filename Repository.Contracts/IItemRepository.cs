using Entities;

namespace Repository.Contracts
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<Item?> GetItemAsync(Guid orderId, Guid productId, bool tracking = false);
    }
}
