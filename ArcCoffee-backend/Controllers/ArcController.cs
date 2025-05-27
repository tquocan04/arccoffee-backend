using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Sprache;
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

        [HttpPut]
        [Authorize(Policy = "StaffOnly")]
        public async Task<IActionResult> UpdateStaffProfile([FromBody] CreateStaffRequest req)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            if (email != req.Email)
            {
                return BadRequest(new Response<string>
                {
                    Message = "Email does not match. Email cannot change."
                });
            }

            await userService.UpdateStaffProfileAsync(email, req);

            return Ok(new Response<string>
            {
                Message = "Successful."
            });
        }

        [Authorize(Policy = "StaffOnly")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStaff()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            await userService.DeleteStaffAsync(email);

            return NoContent();
        }
    }
}
