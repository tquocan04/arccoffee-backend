using DTOs;
using DTOs.Requests;

namespace Service.Contracts
{
    public interface IOrderService
    {
        Task<CartDTO> GetCartAsync(string email);
        Task AddToCartAsync(string id, Guid productId);
        Task DeleteItemInCartAsync(string id, Guid productId);
        Task<CartDTO> MergeCartFromClientAsync(string customerId, List<ItemRequest> req);
        Task UpdateQuantityItemAsync(string customerId, Guid productId, int quantity);
    }
}
