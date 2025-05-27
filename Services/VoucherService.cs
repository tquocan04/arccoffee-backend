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
        IVoucherRepository voucherRepository) 
        : IVoucherService
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

            Voucher voucher = mapper.Map<Voucher>(req);

            voucher.ExpiryDate = new DateOnly(req.Year, req.Month, req.Day);

            if (!voucher.IsValidExpireDate())
                throw new BadRequestInvalidExpiredDateException();

            await voucherRepository.Create(voucher);

            await voucherRepository.Save();

            VoucherDTO result = mapper.Map<VoucherDTO>(voucher);

            return result;
        }

        public async Task<VoucherDTO> GetVoucherByCodeAsync(string code)
        {
            Voucher voucher = await voucherRepository.GetVoucherByCodeAsync(code)
                ?? throw new NotFoundVoucherByCodeException(code);

            if (!voucher.IsValidExpireDate())
                throw new BadRequestInvalidExpiredDateException();

            if (voucher.Quantity <= 0)
                throw new BadRequestVoucherIsOutOfStockException();

            return mapper.Map<VoucherDTO>(voucher);
        }

        public async Task<CreateVoucherRequest> UpdateVoucherAsync(Guid id, CreateVoucherRequest req)
        {
            if (await voucherRepository.CheckCodeVoucherByIdAsync(id, req.Code))
                throw new NotFoundVoucherByCodeException(req.Code);

            Voucher voucher = mapper.Map<Voucher>(req);
            voucher.Id = id;

            voucher.ExpiryDate = new DateOnly(req.Year, req.Month, req.Day);

            if (!voucher.IsValidExpireDate())
                throw new BadRequestInvalidExpiredDateException();

            voucherRepository.Update(voucher);
            await voucherRepository.Save();

            return req;
        }
    }
}
