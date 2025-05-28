using Entities;

namespace Repository.Contracts
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentListAsync(bool tracking = false);
        Task<Payment?> GetPaymentByIdAsync(string id, bool tracking = false);
    }
}
