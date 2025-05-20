using DTOs;
using DTOs.Requests;
using Entities;

namespace Service.Contracts
{
    public interface IProductService
    {
        Task<Product> CreateNewProductAsync(CreateProductRequest req);
        Task<IEnumerable<ProductDTO>> GetProductListAsync(bool? isAvailable);
        Task<ProductDTO> GetProductByIdAsync(Guid id);
        Task UpdateStatusProductByIdAsync(Guid id);
        Task UpdateProductAsync(Guid id, CreateProductRequest req);
    }
}
