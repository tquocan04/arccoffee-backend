using DTOs;
using DTOs.Requests;
using Entities;
using Entities.Context;
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
    }
}
