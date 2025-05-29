using DTOs;
using DTOs.Requests;
using DTOs.Responses;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<(string, string?, string?)> LoginAsync(LoginRequest req);
        Task<(string, string?)> LoginByGoogleAsync(string googleId);
        Task<CustomerResponse> CreateNewCustomerAsync(RegisterRequest req);
        Task<UserDTO> GetProfileAsync(string email);
        Task<UserUpdateDTO> UpdateUserAsync(string email, UpdateUserRequest req);
        Task ChangePasswordAsync(string email, ChangePasswordRequest req);
        Task<CustomerResponse> SignUpGoogleAsync(SignupGoogleRequest req);
        Task<StaffDTO> CreateNewStaffAsync(CreateStaffRequest req);
        Task<StaffDTO> GetStaffProfileAsync(string email);
        Task DeleteStaffAsync(string email);
        Task UpdateStaffProfileAsync(string email, CreateStaffRequest req);
        Task<IEnumerable<StaffDTO>> GetStaffListAsync();
    }
}
