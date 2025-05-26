using Entities;

namespace Repository.Contracts
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetCartByCustomerIdAsync(string customerId, bool tracking = false);
        Task<Order?> GetOrderByIdAsync(Guid id, bool tracking = false);
        Task<Order> GetCartWithoutItemsByCustomerIdAsync(string customerId, bool tracking = false);
    }
}
