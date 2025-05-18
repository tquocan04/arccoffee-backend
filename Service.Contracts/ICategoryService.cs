using DTOs;
using DTOs.Requests;
using Entities;

namespace Service.Contracts
{
    public interface ICategoryService
    {
        Task CreateNewCategoryAsync(CreateCategoryRequest req);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    }
}
