using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/statistic")]
    [ApiController]
    [Authorize(Policy = "AdminAndStaffOnly")]
    public class StatisticController(IStatisticService service) : ControllerBase
    {
        /// <summary>
        /// THỐNG KÊ TOÀN BỘ PHÂN LOẠI: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <param name="year">Năm được yêu cầu</param>
        /// <param name="month">Tháng được yêu cầu. Mặc định là All</param>
        /// <response code="200">Thống kê thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Dữ liệu không tồn tại để thống kê.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpGet]
        public async Task<IActionResult> GetRevenueByYearMonth(int year, string month = "All")
        {
            Dictionary<string, decimal?> result = (month == "All")
                ? await service.GetRevenueCategoriesByYearMonthAsync(year)
                : await service.GetRevenueCategoriesByYearMonthAsync(year, Int32.Parse(month));

            return Ok(new Response<Dictionary<string, decimal?>>
            {
                Message = "Successful.",
                Data = result
            });
        }

        /// <summary>
        /// THỐNG KÊ TOÀN BỘ SẢN PHẨM THEO TỪNG LOẠI: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <param name="categoryId">Id loại được yêu cầu</param>
        /// <param name="year">Năm được yêu cầu</param>
        /// <param name="month">Tháng được yêu cầu. Mặc định là All</param>
        /// <response code="200">Thống kê thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Dữ liệu không tồn tại để thống kê.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [HttpGet("category")]
        public async Task<IActionResult> GetRevenueByCategoryYearMonth(Guid categoryId, int year, string month = "All")
        {
            Dictionary<string, decimal?> result = (month == "All")
                ? await service.GetRevenueByCategoriyInYearMonthAsync(categoryId, year)
                : await service.GetRevenueByCategoriyInYearMonthAsync(categoryId, year, Int32.Parse(month));

            return Ok(new Response<Dictionary<string, decimal?>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
