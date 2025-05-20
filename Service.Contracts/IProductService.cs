using DTOs.Requests;
using Entities;

namespace Service.Contracts
{
    public interface IProductService
    {
        Task<Product> CreateNewProductAsync(CreateProductRequest req);
    }
}
