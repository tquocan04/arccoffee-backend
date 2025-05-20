using DTOs;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace ArcCoffee_backend.Controllers
{
    [Route("api/regions")]
    [ApiController]
    public class RegionController(IRegionService service) : ControllerBase
    {
        private readonly IRegionService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetRegionList()
        {
            var result = await _service.GetAllRegionsAsync();

            return Ok(new Response<IEnumerable<RegionDTO>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
