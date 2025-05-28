using DTOs;
using DTOs.Requests;
using DTOs.Responses;
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
        /// TẠO HÓA ĐƠN: Tạo hóa đơn cho nhân viên mới. Yêu cầu token Customer.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin. voucherCode nếu không có có thể để null hoặc "".
        /// </remarks>
        /// <response code="200">Hóa đơn được tạo thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Đơn hàng không hợp lệ (do giá trị hóa đơn không lớn hơn 0). || Ngày sinh không hợp lệ. Yêu cầu ngày sinh phải trước ngày hiện tại.</response>
        /// <response code="404">Khách hàng || Phương thức thanh toán || Phương thức giao hàng || Voucher || Địa chỉ mặc định không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình tạo nhân viên.</response>
        [HttpPost]

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

            return Ok(new Response<BillDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
