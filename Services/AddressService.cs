using ExceptionHandler.Address;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class AddressService<T>(IAddressRepository address) : IAddressService<T> where T : class
    {
        private readonly IAddressRepository _address = address;

        private static void SetProperty(T obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);

            }
        }

        public async Task<T> SetAddressAsync(T obj, Guid id)
        {
            var address = await _address.GetAddressByObjectIdAsync(id)
                ?? throw new NotFoundAddressException(id);

            var districtId = address.DistrictId;

            var district = await _address.GetDistrictByIdAsync(districtId)
                ?? throw new NotFoundDistrictException();

            var city = await _address.GetCityByDistrictIdAsync(districtId)
                ?? throw new NotFoundCityException();

            var region = await _address.GetRegionByCityIdAsync(city.Id)
                ?? throw new NotFoundRegionException();

            SetProperty(obj, "Street", address.Street);
            SetProperty(obj, "DistrictId", districtId);
            SetProperty(obj, "DistrictName", district.Name ?? "");
            SetProperty(obj, "CityId", city.Id);
            SetProperty(obj, "CityName", city.Name ?? "");
            SetProperty(obj, "RegionId", region.Id);
            SetProperty(obj, "RegionName", region.Name ?? "");

            return obj;
        }
    }
}
