using DTOs;
using ExceptionHandler.General;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class VoucherService(IMapper mapper,
        IVoucherRepository voucher) : IVoucherService
    {
        public async Task<IEnumerable<VoucherDTO>> GetVoucherListAsync()
        {
            var result = await voucher.GetVoucherListAsync();

            if (result == null || !result.Any())
            {
                throw new NotFoundListException();
            }

            return mapper.Map<IEnumerable<VoucherDTO>>(result);
        }
    }
}
