using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Sprache;
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

        [HttpPost]
        public async Task<IActionResult> CustomerRegister([FromForm] RegisterRequest req)
        {
            var result = await _userService.CreateNewCustomerAsync(req);

            return Ok(new Response<CustomerResponse>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
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
