using Entities;

namespace Repository.Contracts
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<Address?> GetAddressByObjectIdAsync(Guid objectId);
        Task<Region?> GetRegionByCityIdAsync(Guid cityId);
        Task<City?> GetCityByDistrictIdAsync(Guid districtId);
        Task<District?> GetDistrictByIdAsync(Guid districtId);
    }
}
