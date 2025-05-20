using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromForm] CreateProductRequest req)
        {
            var result = await _productService.CreateNewProductAsync(req);

            return Ok(new Response<Product>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAvailableProductList()
        {
            var result = await _productService.GetAvailableProductListAsync();

            return Ok(new Response<IEnumerable<ProductDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
