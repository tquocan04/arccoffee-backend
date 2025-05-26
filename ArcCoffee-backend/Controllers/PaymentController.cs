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
