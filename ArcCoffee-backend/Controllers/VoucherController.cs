using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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

        [HttpPost]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> CreateNewVoucher([FromBody] CreateVoucherRequest req)
        {
            VoucherDTO result = await service.CreateNewVoucherAsync(req);

            return CreatedAtAction(nameof(GetAllVouchers), new { id = result.Id },
                new Response<VoucherDTO>
                {
                    Message = "Create successfully.",
                    Data = result
                });
        }

        [HttpGet("detail")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetVoucherByCode([FromQuery] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new Response<string>
                {
                    Message = "Code is null."
                });

            var result = await service.GetVoucherByCodeAsync(code);

            return Ok(new Response<VoucherDTO>
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateVoucher(Guid id, [FromBody] CreateVoucherRequest req)
        {
            var result = await service.UpdateVoucherAsync(id, req);

            return Ok(new Response<CreateVoucherRequest>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucher([FromQuery] string code)
        {
           await service.DeleteVoucherAsync(code);

            return NoContent();
        }
    }
}
