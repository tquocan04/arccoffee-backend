using Entities;

namespace Repository.Contracts
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetCartByCustomerIdAsync(string customerId);
    }
}
