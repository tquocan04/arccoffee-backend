using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/branches")]
    [ApiController]
    [Authorize(Policy = "AdminAndStaffOnly")]
    public class BranchController(IBranchService branchService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateNewBranch([FromBody] CreateBranchRequest req)
        {
            var result = await branchService.CreateNewBranchAsync(req);

            return Ok(new Response<BranchDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
