using DTOs.Requests;

namespace Service.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(LoginRequest req, string role, string id);
    }
}
