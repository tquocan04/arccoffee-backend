using DTOs;
using DTOs.Requests;
using Entities;
using Entities.Context;
using ExceptionHandler.Address;
using ExceptionHandler.Branch;
using ExceptionHandler.General;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class BranchService(IMapper mapper,
        Context context,
        IAddressService<BranchDTO> addressService,
        IBranchRepository branchRepository,
        IAddressRepository addressRepository)
        : IBranchService
    {

        public async Task<BranchDTO> CreateNewBranchAsync(CreateBranchRequest req)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Branch branch = mapper.Map<Branch>(req);

                await branchRepository.Create(branch);

                await branchRepository.Save();

                Console.WriteLine($"branch: {branch.Name}");

                Address address = new()
                {
                    BranchId = branch.Id,
                    IsDefault = true,
                };

                mapper.Map(req, address);

                await addressRepository.Create(address);

                await addressRepository.Save();
                Console.WriteLine($"adress: {address.Id} - {address.BranchId}");

                await transaction.CommitAsync();

                BranchDTO branchDTO = mapper.Map<BranchDTO>(branch);

                branchDTO = await addressService.SetAddressAsync(branchDTO, branch.Id);

                return branchDTO;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IList<BranchDTO>> GetBranchListAsync()
        {
            var branches = await branchRepository.GetBranchListAsync();

            if (branches == null || !branches.Any())
                throw new NotFoundListException();

            var result = mapper.Map<IList<BranchDTO>>(branches);

            var length = result.Count;

            for (int i = 0; i < length; i++)
            {
                result[i] = await addressService.SetAddressAsync(result[i], result[i].Id);
            }

            return result;
        }

        public async Task<BranchDTO> GetBranchByIdAsync(Guid id)
        {
            var branch = await branchRepository.GetBranchByIdAsync(id)
                ?? throw new NotFoundBranchException();

            var result = mapper.Map<BranchDTO>(branch);

            result = await addressService.SetAddressAsync(result, branch.Id);

            return result;
        }

        public async Task DeleteBranchAsync(Guid id)
        {
            var branch = await branchRepository.GetBranchByIdAsync(id)
                ?? throw new NotFoundBranchException();

            branchRepository.Delete(branch);

            await branchRepository.Save();
        }
        
        public async Task UpdateBranchAsync(Guid id, CreateBranchRequest req)
        {
            var branch = await branchRepository.GetBranchByIdAsync(id)
                ?? throw new NotFoundBranchException();

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                mapper.Map(req, branch);

                branchRepository.Update(branch);

                var existingAddress = await addressRepository.GetAddressByObjectIdAsync(id)
                    ?? throw new NotFoundAddressException(id);

                if (existingAddress.DistrictId != req.DistrictId || existingAddress.Street != req.Street)
                {
                    mapper.Map(req, existingAddress);
                    addressRepository.Update(existingAddress);
                }

                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            
        }
    }
}
