using Entities;

namespace Repository.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> IsCategoryExistAsync(string name);
        Task<IEnumerable<Category>> GetCategoryListAsync();
    }
}
