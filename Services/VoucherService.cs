using DTOs;
using DTOs.Requests;
using Entities;
using ExceptionHandler.General;
using ExceptionHandler.Voucher;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class VoucherService(IMapper mapper,
        IVoucherRepository voucherRepository,
        IUserRepository userRepository) : IVoucherService
    {
        public async Task<IEnumerable<VoucherDTO>> GetVoucherListAsync()
        {
            var result = await voucherRepository.GetVoucherListAsync();

            if (result == null || !result.Any())
            {
                throw new NotFoundListException();
            }

            return mapper.Map<IEnumerable<VoucherDTO>>(result);
        }

        public async Task<VoucherDTO> CreateNewVoucherAsync(CreateVoucherRequest req)
        {
            if (await voucherRepository.CheckCodeVoucherAsync(req.Code))
            {
                throw new BadRequestVoucherCodeAlreadyExistsException();
            }

            if (!userRepository.CheckValidDob(req.Day, req.Month, req.Year))
            {
                throw new BadRequestInvalidDateException();
            }

            Voucher voucher = mapper.Map<Voucher>(req);

            voucher.ExpiryDate = new DateOnly(req.Year, req.Month, req.Day);

            await voucherRepository.Create(voucher);

            await voucherRepository.Save();

            VoucherDTO result = mapper.Map<VoucherDTO>(voucher);

            return result;
        }
    }
}
