using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
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

        public AuthenticationController(IUserService userService) 
        {
            _userService = userService;
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
    }
}
