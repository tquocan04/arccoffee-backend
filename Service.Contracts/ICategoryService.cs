using DTOs;
using DTOs.Requests;

namespace Service.Contracts
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateNewCategoryAsync(CreateCategoryRequest req);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(Guid id, bool tracking);
        Task UpdateCategoryAsync(Guid id, CreateCategoryRequest req);
    }
}
