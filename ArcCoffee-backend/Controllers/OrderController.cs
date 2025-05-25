using DTOs;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Services;
using System.Security.Claims;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(Policy = "CustomerOnly")]
    public class OrderController(IOrderService order) : ControllerBase
    {
        private readonly IOrderService _order = order;

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await _order.GetCartAsync(email);

            return Ok(new Response<CartDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }

        [HttpPost("item")]
        public async Task<IActionResult> AddToCart([FromQuery] Guid productId)
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            await _order.AddToCartAsync(id, productId);
            return NoContent();
        }
    }
}
