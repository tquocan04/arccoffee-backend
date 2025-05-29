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
        /// <summary>
        /// LẤY DANH SÁCH VOUCHER: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <response code="200">Lấy danh sách thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không có voucher nào.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// TẠO MỚI VOUCHER: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="201">Tạo thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Mã voucher đã tồn tại || Thời hạn không hợp lý.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// LẤY CHI TIẾT VOUCHER: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <param name="code">Mã code voucher được yêu cầu.</param>
        /// <response code="200">Lấy kết quả thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Voucher không tồn tại.</response>
        /// <response code="400">Số lượng đã hết || Thời gian đã hết hạn.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// LẤY DANH SÁCH VOUCHER.
        /// </summary>
        /// <remarks>
        /// Public cho toàn bộ người dùng.
        /// </remarks>
        /// <response code="200">Lấy danh sách thành công.</response>
        /// <response code="404">Không có voucher nào.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// CẬP NHẬT VOUCHER: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="200">Thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Thời hạn không hợp lý.</response>
        /// <response code="404">Không có voucher đang được yêu cầu.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// XÓA VOUCHER: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <param name="code">Mã code voucher được yêu cầu.</param>
        /// <response code="204">Thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không có voucher đang được yêu cầu.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucher([FromQuery] string code)
        {
           await service.DeleteVoucherAsync(code);

            return NoContent();
        }
    }
}
