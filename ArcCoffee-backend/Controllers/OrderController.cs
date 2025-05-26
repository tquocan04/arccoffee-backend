using DTOs;
using DTOs.Requests;
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
        
        [HttpDelete("item")]
        public async Task<IActionResult> DeleteItem([FromQuery] Guid productId)
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            await _order.DeleteItemInCartAsync(id, productId);
            return NoContent();
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemList([FromBody] List<ItemRequest> req)
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            if (req == null || req.Count <= 0)
            {
                return BadRequest(new Response<string>
                {
                    Message = "Product list is empty."
                });
            }

            CartDTO cart = await _order.MergeCartFromClientAsync(id, req);

            return Ok(new Response<CartDTO>
            {
                Message = "Products is merged successfully.",
                Data = cart
            });

        }
    }
}
