using DTOs.Requests;
using DTOs.Responses;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/category")]
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
    }
}
