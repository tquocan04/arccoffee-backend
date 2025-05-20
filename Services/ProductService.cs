using DTOs;
using DTOs.Requests;
using Entities;
using ExceptionHandler.Category;
using ExceptionHandler.General;
using ExceptionHandler.Product;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;
using Services.Extensions;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly CloudinaryService _cloudinary;

        public ProductService(IMapper mapper,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            CloudinaryService cloudinary)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _cloudinary = cloudinary;
        }

        public async Task<Product> CreateNewProductAsync(CreateProductRequest req)
        {
            if (!string.IsNullOrEmpty(req.Name))
            {
                if (await _productRepository.ProductExistsByNameAsync(req.Name))
                {
                    throw new BadRequestProductExistsByNameException(req.Name);
                }
            }

            var newProduct = _mapper.Map<Product>(req);

            if (req.Image != null && req.Image.Length != 0)
            {
                var url = await _cloudinary.UploadImageProductAsync(newProduct, req.Image);
                newProduct.Image = url;
            }

            await _productRepository.Create(newProduct);

            await _productRepository.Save();

            return newProduct;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductListAsync(bool? isAvailable)
        {
            var list = await _productRepository.GetProductListAsync(isAvailable);

            if (list == null || !list.Any())
            {
                throw new NotFoundListException();
            }

            var result = _mapper.Map<IList<ProductDTO>>(list);

            int length = result.Count;

            for (int i = 0; i < length; i++)
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(list[i].CategoryId, false)
                    ?? throw new NotFoundCategoryException(list[i].CategoryId);

                result[i].CategoryName = category.Name;
            }

            return result;
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id)
                ?? throw new NotFoundProductException(id);

            var result = _mapper.Map<ProductDTO>(product);

            var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId, false)
                ?? throw new NotFoundCategoryException(product.CategoryId);

            result.CategoryName = category.Name;

            return result;
        }
        
        public async Task UpdateStatusProductByIdAsync(Guid id)
        {
            var result = await _productRepository.GetProductByIdAsync(id, true)
                ?? throw new NotFoundProductException(id);

            if (result.IsAvailable)
                result.IsAvailable = false;
            else
                result.IsAvailable = true;

            await _productRepository.Save();
        }
    }
}
