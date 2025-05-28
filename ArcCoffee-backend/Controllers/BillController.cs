using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/bills")]
    [ApiController]
    public class BillController(IBillService billService) : ControllerBase
    {
        /// <summary>
        /// TẠO HÓA ĐƠN: Tạo hóa đơn cho khách hàng. Yêu cầu token Customer.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin. voucherCode nếu không có có thể để null hoặc "".
        /// </remarks>
        /// <response code="201">Hóa đơn được tạo thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Đơn hàng không hợp lệ (do giá trị hóa đơn không lớn hơn 0). || Ngày sinh không hợp lệ. Yêu cầu ngày sinh phải trước ngày hiện tại.</response>
        /// <response code="404">Khách hàng || Phương thức thanh toán || Phương thức giao hàng || Voucher || Địa chỉ mặc định không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpPost]
        [Authorize(Policy = "CustomerOnly")]

        public async Task<IActionResult> CreateNewBill([FromBody] BillRequest req)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await billService.CreateNewBillAsync(email, req);

            return CreatedAtAction(nameof(GetBillDetail),
                new { id = result.Id },
                new Response<BillDTO>
                {
                    Message = "Successful.",
                    Data = result
                });
        }

        /// <summary>
        /// LẤY DANH SÁCH HÓA ĐƠN: Yêu cầu token xác thực cho toàn bộ.
        /// </summary>
        /// <param name="status">staus được yêu cầu (Pending và Completed). Mặc định là Pending</param>
        /// <response code="200">Danh sách được lấy thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không tìm thấy người dùng || kết quả.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpGet]
        [Authorize(Policy = "All")]

        public async Task<IActionResult> GetBillList([FromQuery] string status = "Pending")
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            string? role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            IEnumerable<BillDTO> result = (role == "Customer") ?
                await billService.GetBillListAsync(email, status)
                : await billService.GetBillListAsync(null, status);

            return Ok(new Response<IEnumerable<BillDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }

        /// <summary>
        /// CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG: Yêu cầu token cho Admin và Staff.
        /// </summary>
        /// <param name="id">id đơn hàng được yêu cầu.</param>
        /// <response code="204">Cập nhật thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không tìm thấy đơn hàng.</response>
        /// <response code="400">Trạng thái đơn hàng không đúng (cần phải trong trạng thái Pending).</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpPut]
        [Authorize(Policy = "AdminAndStaffOnly")]
        public async Task<IActionResult> UpdateStatusBill([FromQuery] Guid id)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            string? role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            await billService.UpdateStatusBillAsync(id);

            return NoContent();
        }

        /// <summary>
        /// LẤY CHI TIẾT ĐƠN HÀNG: Yêu cầu token cho toàn bộ.
        /// </summary>
        /// <param name="id">id đơn hàng được yêu cầu.</param>
        /// <response code="200">Lấy đơn hàng thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không tìm thấy đơn hàng.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpGet("{id:guid}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetBillDetail(Guid id)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;
            string? role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            BillDTO result = await billService.GetBillByIdAsync(id);

            return Ok(new Response<BillDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
