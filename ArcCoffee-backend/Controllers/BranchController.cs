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
    public class BranchController(IBranchService branchService) : ControllerBase
    {
        [Authorize(Policy = "AdminAndStaffOnly")]
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

        [HttpGet]
        public async Task<IActionResult> GetBranchList()
        {
            var result = await branchService.GetBranchListAsync();

            return Ok(new Response<IList<BranchDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBranchDetail(Guid id)
        {
            var result = await branchService.GetBranchByIdAsync(id);

            return Ok(new Response<BranchDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
