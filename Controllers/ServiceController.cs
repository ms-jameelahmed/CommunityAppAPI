using CommunityAppAPI.Models;
using CommunityAppAPI.Services.Common;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _service;
        private readonly IResponseService _responseService;

        public ServiceController(IServiceService service, IResponseService responseService)
        {
            _service = service;
            _responseService = responseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceMaster model)
        {
            var id = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(Get), new { id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ServiceMaster model)
        {
            if (id != model.ServiceId) return BadRequest();
            var success = await _service.UpdateAsync(model);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("ExploreServices")]
        public async Task<IActionResult> GetExploreServices(
       [FromQuery] string? search,
       [FromQuery] string? sortBy,
       [FromQuery] decimal? minPrice,
       [FromQuery] decimal? maxPrice,
       [FromQuery] long? servicesId, [FromQuery] int? pagenumber,[FromQuery] int? pagesize)
        {
            try
            {
                var services = await _service.GetExploreServicesAsync(search, sortBy, minPrice, maxPrice, servicesId,pagenumber, pagesize);
                return _responseService.SuccessResponse(services, "Explore services fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching explore services: {ex.Message}");
            }
        }
    }

}
