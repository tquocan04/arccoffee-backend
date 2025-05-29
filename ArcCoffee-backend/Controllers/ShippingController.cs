using DTOs;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/shippings")]
    [ApiController]
    public class ShippingController(IShippingService shippingService) : ControllerBase
    {
        private readonly IShippingService _shippingService = shippingService;

        /// <summary>
        /// LẤY DANH SÁCH PHƯƠNG THỨC GIAO HÀNG.
        /// </summary>
        /// <response code="200">Thành công.</response>
        /// <response code="404">Không có kết quả.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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
