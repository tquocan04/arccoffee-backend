using DTOs;

namespace Service.Contracts
{
    public interface IOrderService
    {
        Task<CartDTO> GetCartAsync(string email);
    }
}
