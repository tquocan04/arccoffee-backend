using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class AddressRepository(Context context) : BaseRepository<Address>(context), IAddressRepository
    {
        private readonly Context _context = context;

        public async Task<Address?> GetAddressByObjectIdAsync(Guid objectId)
        {
            return await _context.Addresses.AsNoTracking()
               .FirstOrDefaultAsync(a => (a.UserId == objectId.ToString()
                                       || a.BranchId == objectId)
                                       && a.IsDefault);
        }

        public async Task<Region?> GetRegionByCityIdAsync(Guid cityId)
        {
            var city = await _context.Cities.AsNoTracking()
                                        .FirstOrDefaultAsync(c => c.Id == cityId);

            if (city == null)
            {
                return null;
            }

            return await _context.Regions.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == city.RegionId);
        }
        
        public async Task<City?> GetCityByDistrictIdAsync(Guid districtId)
        {
            var district = await _context.Districts.AsNoTracking()
                                        .FirstOrDefaultAsync(d => d.Id == districtId);

            if (district == null)
            {
                return null;
            }

            return await _context.Cities.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == district.CityId);
        }
        
        public async Task<District?> GetDistrictByIdAsync(Guid districtId)
        {
            return await _context.Districts.AsNoTracking()
                                        .FirstOrDefaultAsync(d => d.Id == districtId);
        }

        public async Task<List<Address>> GetListAddressOfCustomerAsync(string customerId)
        {
            return await _context.Addresses
                .AsNoTracking()
                .Where(a => !a.IsDefault &&
                        a.UserId == customerId)
                .ToListAsync();
        }
    }
}
