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
            var result = await _categoryService.CreateNewCategoryAsync(req);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = result.Id },
                new Response<CategoryDTO>
                {
                    Message = "Successful.",
                    Data = result,
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id, false);

            return Ok(new Response<CategoryDTO>
            {
                Message = "Successful.",
                Data = result,
            });
        }
    }
}
