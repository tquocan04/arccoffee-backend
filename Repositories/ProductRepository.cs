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

        public async Task<IList<Product>> GetProductListAsync(bool isAvailable)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsAvailable == isAvailable)
                .ToListAsync();
        }
        
        public async Task<Product?> GetProductByIdAsync(Guid id, bool tracking = false)
        {
            if (tracking)
            {
                return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            }

            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
