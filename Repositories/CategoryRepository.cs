using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly Context _context;

        public CategoryRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsCategoryExistAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Category>> GetCategoryListAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<Category?> GetCategoryByIdAsync(Guid id, bool tracking)
        {
            if (tracking)
            {
                return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
            }

            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
