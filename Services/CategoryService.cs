using DTOs;
using DTOs.Requests;
using Entities;
using ExceptionHandler.Category;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDTO> CreateNewCategoryAsync(CreateCategoryRequest req)
        {
            if (await _categoryRepository.IsCategoryExistAsync(req.Name))
            {
                throw new BadRequestCategoryNameIsExistedException(req.Name);
            }

            var category = _mapper.Map<Category>(req);

            await _categoryRepository.Create(category);

            await _categoryRepository.Save();

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var result = await _categoryRepository.GetCategoryListAsync();

            if (result == null || !result.Any())
            {
                throw new NotFoundCategoryListException();
            }

            return _mapper.Map<IEnumerable<CategoryDTO>>(result);
        }
        
        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid id, bool tracking)
        {
            var result = await _categoryRepository.GetCategoryByIdAsync(id, tracking)
                ?? throw new NotFoundCategoryException(id);
            
            return _mapper.Map<CategoryDTO>(result);
        }
    }
}
