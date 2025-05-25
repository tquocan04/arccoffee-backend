using DTOs;
using DTOs.Requests;
using DTOs.Responses;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<CustomerResponse> CreateNewCustomerAsync(RegisterRequest req);
        Task<(string?, string?)> LoginAsync(LoginRequest req);
        Task<UserDTO> GetProfileAsync(string email);
        Task<UserUpdateDTO> UpdateUserAsync(string email, UpdateUserRequest req);
        Task ChangePasswordAsync(string email, ChangePasswordRequest req);
    }
}
