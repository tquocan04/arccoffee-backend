using DTOs.Requests;
using DTOs.Responses;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<CustomerResponse> CreateNewCustomerAsync(RegisterRequest req);
        Task<(string?, string?)> LoginAsync(LoginRequest req);
    }
}
