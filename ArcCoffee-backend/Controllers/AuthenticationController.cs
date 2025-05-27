using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        /// ĐĂNG KÝ TÀI KHOẢN BẰNG GOOGLE: Tạo tài khoản bằng google cho khách hàng mới.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="201">Khách hàng mới được tạo thành công.</response>
        /// <response code="400">Email đã tồn tại. || Ngày sinh không hợp lệ. Yêu cầu ngày sinh phải trước ngày hiện tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình tạo.</response>
        [HttpPost("google")]
        public async Task<IActionResult> SignUpGoogle([FromBody] SignupGoogleRequest req)
        {
            var result = await _userService.SignUpGoogleAsync(req);

            return CreatedAtAction(nameof(GetProfile),
                new { id = result.Id },
                new Response<CustomerResponse>
                {
                    Message = "Successful.",
                    Data = result
                });
        }

        /// <summary>
        /// ĐĂNG KÝ TÀI KHOẢN: Tạo tài khoản cho khách hàng mới.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="201">Khách hàng mới được tạo thành công.</response>
        /// <response code="400">Email đã tồn tại. || Ngày sinh không hợp lệ. Yêu cầu ngày sinh phải trước ngày hiện tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình tạo.</response>
        [HttpPost]
        public async Task<IActionResult> CustomerRegister([FromForm] RegisterRequest req)
        {
            var result = await _userService.CreateNewCustomerAsync(req);

            return CreatedAtAction(nameof(GetProfile),
                new { id = result.Id },
                new Response<CustomerResponse>
                {
                    Message = "Successful.",
                    Data = result
                });
        }

        /// <summary>
        /// LẤY THÔNG TIN: Lấy thông tin của khách hàng. Yêu cầu token Customer.
        /// </summary>
        /// <response code="200">Thông tin được lấy thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Khách hàng không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình lấy thông tin.</response>
        [Authorize(Policy = "CustomerOnly")]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await _userService.GetProfileAsync(email);

            return Ok(new Response<UserDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }

        /// <summary>
        /// CẬP NHẬT THÔNG TIN: Khách hàng cập nhật thông tin. Yêu cầu token Customer.
        /// </summary>
        /// <remarks>
        /// Yêu cầu điền đầy đủ thông tin. Ảnh nếu không update có thể để null.
        /// </remarks>
        /// <response code="200">Thông tin được cập nhật thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Ngày sinh không hợp lệ. Yêu cầu ngày sinh phải trước ngày hiện tại.</response>
        /// <response code="404">Khách hàng không tồn tại. || Địa chỉ không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình cập nhật.</response>
        [Authorize(Policy = "CustomerOnly")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserRequest req)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await _userService.UpdateUserAsync(email, req);

            return Ok(new Response<UserUpdateDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }

        /// <summary>
        /// KIỂM TRA EMAIL TỒN TẠI
        /// </summary>
        /// <param name="email">Email yêu cầu.</param>
        /// <response code="200">Email hợp lệ.</response>
        /// <response code="400">Email không hợp lệ.</response>
        /// <response code="500">Đã có lỗi trong quá trình kiểm tra.</response>
        [HttpGet("email")]
        public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            if (result == null)
            {
                return BadRequest(new Response<string>
                {
                    Message = "Invalid email!"
                });
            }

            return Ok(new Response<string>
            {
                Message = "Valid email!"
            });
        }

        /// <summary>
        /// THAY ĐỔI MẬT KHẨU. Yêu cầu xác thực cho toàn bộ người dùng.
        /// </summary>
        /// <remarks>
        /// Cung cấp mật khẩu hiện tại và mật khẩu mới.
        /// </remarks>
        /// <response code="200">Thay đổi thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Người dùng không tồn tại.</response>
        /// <response code="400">Thay đổi thất bại. || Mật khẩu hiện tại không hợp lệ.</response>
        /// <response code="500">Đã có lỗi trong quá trình thay đổi.</response>
        [Authorize(Policy = "All")]
        [HttpPut("password")]
        public async Task<IActionResult> UpdateNewPassword([FromBody] ChangePasswordRequest req)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Invalid token!"
                });
            }

            await _userService.ChangePasswordAsync(email, req);

            return Ok(new Response<string>
            {
                Message = "Successful."
            });
        }
    }
}
