using DTOs;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/shippings")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShippings()
        {
            var result = await _shippingService.GetShippingListAsync();

            return Ok(new Response<IEnumerable<ShippingDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
