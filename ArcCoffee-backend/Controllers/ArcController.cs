using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/arc")]
    [ApiController]
    public class ArcController(IUserService userService) : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateNewStaff([FromBody] CreateStaffRequest req)
        {
            var result = await userService.CreateNewStaffAsync(req);

            return Ok(new Response<StaffDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [Authorize(Policy = "StaffOnly")]
        [HttpGet]
        public async Task<IActionResult> GetStaffProfile()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await userService.GetStaffProfileAsync(email);

            return Ok(new Response<StaffDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
