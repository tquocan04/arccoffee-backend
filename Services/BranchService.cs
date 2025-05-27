using DTOs;
using DTOs.Requests;
using Entities;
using Entities.Context;
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
    }
}
