using DTOs.Requests;
using Entities;
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
        private readonly CloudinaryService _cloudinary;

        public ProductService(IMapper mapper,
            IProductRepository productRepository,
            CloudinaryService cloudinary)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cloudinary = cloudinary;
        }

        public async Task<Product> CreateNewProductAsync(CreateProductRequest req)
        {
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
    }
}
