using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        /// <summary>
        /// THÊM MỚI LOẠI SẢN PHẨM: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="201">Tạo thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Tên loại của yêu cầu đã tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// LẤY DANH SÁCH PHÂN LOẠI
        /// </summary>
        /// <response code="200">Thành công.</response>
        /// <response code="404">Không có loại nào.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// LẤY CHI TIẾT LOẠI SẢN PHẨM
        /// </summary>
        /// <remarks>
        /// Cung cấp id của loại.
        /// </remarks>
        /// <response code="200">Lấy kết quả thành công.</response>
        /// <response code="404">Loại không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
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

        /// <summary>
        /// CẬP NHẬT LOẠI SẢN PHẨM: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="200">Thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không có loại sản phẩm đang được yêu cầu.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CreateCategoryRequest req)
        {
            await _categoryService.UpdateCategoryAsync(id, req);

            return Ok(new Response<string>
            {
                Message = "Successful.",
            });
        }
    }
}
