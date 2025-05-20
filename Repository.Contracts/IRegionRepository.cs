using Entities;

namespace Repository.Contracts
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
    }
}
