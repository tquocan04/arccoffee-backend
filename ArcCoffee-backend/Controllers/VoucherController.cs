using DTOs;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Services;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/vouchers")]
    [ApiController]
    public class VoucherController(IVoucherService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetAllVouchers()
        {
            var result = await service.GetVoucherListAsync();

            return Ok(new Response<IEnumerable<VoucherDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetAllVouchersPublic()
        {
            var result = await service.GetVoucherListAsync();

            return Ok(new Response<IEnumerable<VoucherDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
