using DTOs;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        /// <summary>
        /// LẤY DANH SÁCH PHƯƠNG THỨC THANH TOÁN.
        /// </summary>
        /// <response code="200">Thành công.</response>
        /// <response code="404">Không có kết quả.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await paymentService.GetPaymentListAsync();

            return Ok(new Response<IEnumerable<PaymentDTO>>
            {
                Message = "Successful.",
                Data = payments
            });
        }
    }
}
