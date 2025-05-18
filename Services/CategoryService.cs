using DTOs.Requests;
using Entities;
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

        public async Task CreateNewCategoryAsync(CreateCategoryRequest req)
        {
            var category = _mapper.Map<Category>(req);

            await _categoryRepository.Create(category);

            await _categoryRepository.Save();
        }
    }
}
