using Entities;
using Entities.Context;
using Repository.Contracts;

namespace Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly Context _context;
        public ProductRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
