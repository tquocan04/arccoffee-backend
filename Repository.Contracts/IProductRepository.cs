using Entities;

namespace Repository.Contracts
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<bool> ProductExistsByNameAsync(string name);
        Task<IList<Product>> GetProductListAsync(bool isAvailable);
        Task<Product?> GetProductByIdAsync(Guid id, bool tracking = false);
    }
}
