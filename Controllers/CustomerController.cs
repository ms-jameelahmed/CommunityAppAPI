using CommunityAppAPI.Models;
using CommunityAppAPI.Services.Common;
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
        private readonly ICustomerService _customerService;
        private readonly IResponseService _responseService;

        public CustomerController(ICustomerService customerService, IResponseService responseService)
        {
            _customerService = customerService;
            _responseService = responseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                customer.CreatedDate = DateTime.Now;
                var resultMessage = await _customerService.AddAsync(customer);

                if (!string.IsNullOrEmpty(resultMessage))
                    return _responseService.ConflictResponse(resultMessage); // 409 Conflict

                return _responseService.SuccessResponse(null, "Customer created successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating customer: {ex.Message}");
            }
        }

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                    return _responseService.NotFoundResponse("Customer not found");

                return _responseService.SuccessResponse(customer, "Customer fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error retrieving customer: {ex.Message}");
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                return _responseService.SuccessResponse(customers, "Customer list fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error retrieving customers: {ex.Message}");
            }
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                customer.CustomerId = id;
                customer.ModifiedDate = DateTime.Now;
                await _customerService.UpdateAsync(customer);
                return _responseService.SuccessResponse(customer, "Customer updated successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error updating customer: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] string modifiedBy)
        {
            try
            {
                await _customerService.DeleteAsync(id, modifiedBy);
                return _responseService.SuccessResponse(null, "Customer deleted successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error deleting customer: {ex.Message}");
            }
        }
    }
}
