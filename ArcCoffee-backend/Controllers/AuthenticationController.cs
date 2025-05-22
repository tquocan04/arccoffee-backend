using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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
    }
}
