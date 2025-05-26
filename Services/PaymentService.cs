using DTOs;
using ExceptionHandler.General;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class PaymentService(IMapper mapper,
        IPaymentRepository payment) : IPaymentService
    {
        public async Task<IEnumerable<PaymentDTO>> GetPaymentListAsync()
        {
            var result = await payment.GetPaymentListAsync();

            if (result == null || !result.Any())
                throw new NotFoundListException();

            return mapper.Map<IEnumerable<PaymentDTO>>(result);
        }
    }
}
