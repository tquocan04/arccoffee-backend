using Entities;
using Entities.Context;
using Repository.Contracts;

namespace Repositories
{
    public class AddressRepository(Context context) : BaseRepository<Address>(context), IAddressRepository
    {
        private readonly Context _context = context;
    }
}
