using DTOs;

namespace Service.Contracts
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetPaymentListAsync();
    }
}
