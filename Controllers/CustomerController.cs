using CommunityAppAPI.Models;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerService service, ILogger<CustomerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private ObjectResult CustomResponse(object data, string message, bool success = true)
        {
            var uniqueId = Guid.NewGuid();
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            _logger.LogInformation("Success: {Message}, UniqueId: {UniqueId}, Timestamp: {Timestamp}",
                message, uniqueId, timestamp);

            return Ok(new
            {
                success,
                message,
                uniqueId,
                timestamp,
                data
            });
        }

        private ObjectResult ErrorResponse(string errorMessage)
        {
            var uniqueId = Guid.NewGuid();
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            _logger.LogError("Error: {Error}, UniqueId: {UniqueId}, Timestamp: {Timestamp}",
                errorMessage, uniqueId, timestamp);

            return StatusCode(500, new
            {
                success = false,
                message = errorMessage,
                uniqueId,
                timestamp,
                data = (object)null
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _service.GetAllAsync();
                return CustomResponse(customers, "Customer list fetched successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse($"Error retrieving customers: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var customer = await _service.GetByIdAsync(id);
                if (customer == null)
                    return NotFound(new
                    {
                        success = false,
                        message = "Customer not found",
                        uniqueId = Guid.NewGuid(),
                        data = (object)null
                    });

                return CustomResponse(customer, "Customer fetched successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse($"Error retrieving customer: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    uniqueId = Guid.NewGuid(),
                    data = ModelState
                });
            }

            try
            {
                customer.CreatedDate = DateTime.Now;
                await _service.AddAsync(customer);
                return CustomResponse(null, "Customer created successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse($"Error creating customer: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    uniqueId = Guid.NewGuid(),
                    data = ModelState
                });
            }

            try
            {
                customer.CustomerId = id;
                customer.ModifiedDate = DateTime.Now;
                await _service.UpdateAsync(customer);
                return CustomResponse(null, "Customer updated successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse($"Error updating customer: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] string modifiedBy)
        {
            try
            {
                await _service.DeleteAsync(id, modifiedBy);
                return CustomResponse(null, "Customer deleted successfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse($"Error deleting customer: {ex.Message}");
            }
        }
    }
}
