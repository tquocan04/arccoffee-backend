using Entities;

namespace Repository.Contracts
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetCartByCustomerIdAsync(string customerId);
        Task<Item?> GetItemAsync(Guid orderId, Guid productId);
        void AddItemToCart(Item item);
        void UpdateQuantityItemToCart(Item item);
        Task<Order?> GetOrderByIdAsync(Guid id, bool tracking = false);
    }
}
