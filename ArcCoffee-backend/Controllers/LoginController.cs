using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/authentication")]
    [ApiController]

    public class LoginController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _userService.LoginAsync(req);

            if (result.Item1 == null && result.Item2 == null)
            {
                return BadRequest(new Response<string>
                {
                    Message = "Login failed."
                });
            }

            return Ok(new LoginResponse
            {
                AccessToken = $"Bearer {result.Item1}",
                Picture = result.Item2,
            });
        }
    }
}
