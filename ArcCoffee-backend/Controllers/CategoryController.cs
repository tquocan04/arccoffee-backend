using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCategory([FromBody] CreateCategoryRequest req)
        {
            await _categoryService.CreateNewCategoryAsync(req);

            return Ok(new Response<CreateCategoryRequest>
            {
                Message = "Successful.",
                Data = req,
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            return Ok(new Response<IEnumerable<CategoryDTO>>
            {
                Message = "Successful.",
                Data = result,
            });
        }
    }
}
