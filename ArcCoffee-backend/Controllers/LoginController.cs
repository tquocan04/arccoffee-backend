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
        /// <summary>
        /// ĐĂNG NHẬP: Đăng nhập với email và mật khẩu
        /// </summary>
        /// <response code="200">Người dùng đăng nhập thành công.</response>
        /// <response code="400">Email hoặc mật khẩu sai.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _userService.LoginAsync(req);

            return Ok(new LoginResponse
            {
                AccessToken = $"Bearer {result.Item1}",
                Picture = result.Item2,
            });
        }

        /// <summary>
        /// ĐĂNG NHẬP: Đăng nhập với google
        /// </summary>
        /// <param name="googleId">GoogleId được yêu cầu.</param>
        /// <response code="200">Người dùng đăng nhập thành công.</response>
        /// <response code="404">Không tìm thấy người dùng với GoogleId được yêu cầu.</response>
        [HttpPost("login/google")]
        public async Task<IActionResult> LoginByGoogle([FromQuery] string googleId)
        {
            var result = await _userService.LoginByGoogleAsync(googleId);

            return Ok(new LoginResponse
            {
                AccessToken = $"Bearer {result.Item1}",
                Picture = result.Item2,
            });
        }
    }
}
