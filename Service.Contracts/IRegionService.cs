using DTOs;

namespace Service.Contracts
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDTO>> GetAllRegionsAsync();
    }
}
