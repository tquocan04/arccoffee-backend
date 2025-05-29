using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/arc")]
    [ApiController]
    public class ArcController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// TẠO TÀI KHOẢN STAFF: Tạo tài khoàn cho nhân viên mới. Yêu cầu token Admin.
        /// </summary>
        /// <remarks>
        /// Cần điền đầy đủ thông tin.
        /// </remarks>
        /// <response code="201">Nhân viên mới được tạo thành công.</response>
        /// <response code="400">Email đã tồn tại. || Ngày sinh không hợp lệ. Yêu cầu ngày sinh phải trước ngày hiện tại.</response>
        /// <response code="404">Branch của nhân viên không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình tạo nhân viên.</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateNewStaff([FromBody] CreateStaffRequest req)
        {
            var result = await userService.CreateNewStaffAsync(req);

            return CreatedAtAction(nameof(GetStaffProfile),
                new { id = result.Id },
                new Response<StaffDTO>
                {
                    Message = "Successful.",
                    Data = result
                });
        }

        /// <summary>
        /// LẤY THÔNG TIN NHÂN VIÊN: Lấy thông tin của nhân viên. Yêu cầu token Staff.
        /// </summary>
        /// <response code="200">Thông tin được lấy thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Nhân viên không tồn tại. || Thông tin chi nhánh của nhân viên không tồn tại hoặc đang bị rỗng.</response>
        /// <response code="500">Đã có lỗi trong quá trình lấy thông tin.</response>
        [Authorize(Policy = "StaffOnly")]
        [HttpGet]
        public async Task<IActionResult> GetStaffProfile()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await userService.GetStaffProfileAsync(email);

            return Ok(new Response<StaffDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }

        /// <summary>
        /// CẬP NHẬT THÔNG TIN: Nhân viên cập nhật thông tin. Yêu cầu token Staff.
        /// </summary>
        /// <remarks>
        /// Email phải giữ nguyên không được thay đổi. Password không được để trống, nhưng cũng sẽ không cập nhật password ở api này (cập nhật tại api/authentication/password).
        /// </remarks>
        /// <response code="200">Thông tin được cập nhật thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="400">Email đã bị thay đổi.</response>
        /// <response code="404">Nhân viên không tồn tại. || Thông tin chi nhánh của nhân viên không tồn tại hoặc đang bị rỗng.</response>
        /// <response code="500">Đã có lỗi trong quá trình cập nhật thông tin.</response>
        [HttpPut]
        [Authorize(Policy = "StaffOnly")]
        public async Task<IActionResult> UpdateStaffProfile([FromBody] CreateStaffRequest req)
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            if (email != req.Email)
            {
                return BadRequest(new Response<string>
                {
                    Message = "Email does not match. Email cannot change."
                });
            }

            await userService.UpdateStaffProfileAsync(email, req);

            return Ok(new Response<string>
            {
                Message = "Successful."
            });
        }

        /// <summary>
        /// XÓA NHÂN VIÊN: Nhân viên tự xóa. Yêu cầu token Staff.
        /// </summary>
        /// <response code="204">Xóa thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Nhân viên không tồn tại.</response>
        /// <response code="500">Đã có lỗi trong quá trình xóa.</response>
        [Authorize(Policy = "StaffOnly")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStaff()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            await userService.DeleteStaffAsync(email);

            return NoContent();
        }
        
        /// <summary>
        /// LẤY DANH SÁCH NHÂN VIÊN: Yêu cầu token Admin và Staff.
        /// </summary>
        /// <response code="200">Lấy danh sách thành công.</response>
        /// <response code="401">Thông tin xác thực thất bại.</response>
        /// <response code="403">Quyền xác thực không đúng.</response>
        /// <response code="404">Không có nhân viên nào.</response>
        /// <response code="500">Đã có lỗi trong quá trình thực hiện.</response>
        [Authorize(Policy = "AdminAndStaffOnly")]
        [HttpGet("staffs")]
        public async Task<IActionResult> GetStaffList()
        {
            string? email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new Response<string>
                {
                    Message = "Unable to authenticate user"
                });
            }

            var result = await userService.GetStaffListAsync();

            return Ok(new Response<IEnumerable<StaffDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
