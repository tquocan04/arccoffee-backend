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

            return CreatedAtAction(nameof(GetProductById), new { id = result.Id },
                new Response<Product>
                {
                    Message = "Successful.",
                    Data = result
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetProductList()
        {
            var result = await _productService.GetProductListAsync(null);

            return Ok(new Response<IEnumerable<ProductDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableProductList()
        {
            var result = await _productService.GetProductListAsync(true);

            return Ok(new Response<IEnumerable<ProductDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [HttpGet("hidden")]
        public async Task<IActionResult> GetHiddenProductList()
        {
            var result = await _productService.GetProductListAsync(false);

            return Ok(new Response<IEnumerable<ProductDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return Ok(new Response<ProductDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> GUpdateStatusProductById(Guid id)
        {
            await _productService.UpdateStatusProductByIdAsync(id);

            return Ok(new Response<string>
            {
                Message = "Status updated successfully.",
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateInforProduct(Guid id, [FromForm] CreateProductRequest req)
        {
            await _productService.UpdateProductAsync(id, req);

            return Ok(new Response<string>
            {
                Message = "Product updated successfully"
            });
        }
    }
}
