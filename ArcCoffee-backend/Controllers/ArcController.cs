using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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
    }
}
