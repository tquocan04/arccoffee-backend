using DTOs.Requests;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class TokenService : ITokenService
    {
        private static List<Claim> GetClaim(LoginRequest req, string role, string id)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, id),
                new (ClaimTypes.Name, req.Login),
                new (ClaimTypes.Role, role),
            };
            return claims;
        }

        public string GenerateToken(LoginRequest req, string role, string id)
        {
            var expiry = int.Parse(Environment.GetEnvironmentVariable("Jwt_TokenExpired"));
            var secret = Environment.GetEnvironmentVariable("Jwt_Secret");
            var issuer = Environment.GetEnvironmentVariable("Jwt_Issuer");
            var audience = Environment.GetEnvironmentVariable("Jwt_Audience");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: GetClaim(req, role, id),
                expires: DateTime.Now.AddMonths(expiry),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
