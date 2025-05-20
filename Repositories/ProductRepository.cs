using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ProductExistsByNameAsync(string name)
        {
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.Name == name);
        }
    }
}
