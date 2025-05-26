using DTOs;

namespace Service.Contracts
{
    public interface IOrderService
    {
        Task<CartDTO> GetCartAsync(string email);
        Task AddToCartAsync(string id, Guid productId);
        Task DeleteItemInCartAsync(string id, Guid productId);
    }
}
