using CommunityAppAPI.Models;
using CommunityAppAPI.Services;
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
        
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                customer.CreatedDate = DateTime.Now;
                var resultMessage = await _customerService.AddAsync(customer);

                if (!resultMessage.Success)
                    return _responseService.ConflictResponse(resultMessage.Message); // 409 Conflict    

                return _responseService.SuccessResponse(null, "Customer created successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating customer: {ex.Message}");
            }
        }
        
        [HttpGet("getCustomerById/{id:long}")]
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
        
        [HttpGet("getallCustomers")]
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
        
        [HttpPost("updatecustomer")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomer customer)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
               
                customer.ModifiedDate = DateTime.Now;
                await _customerService.UpdateAsync(customer);
                return _responseService.SuccessResponse(customer, "Customer updated successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error updating customer: {ex.Message}");
            }
        }

        [HttpPost("deletecustomer")]
        
        public async Task<IActionResult> Delete([FromBody] DeleteCustomer customer)
        {
            try
            {
                await _customerService.DeleteAsync(customer.CustomerId, customer.ModifiedBy );
                return _responseService.SuccessResponse(null, "Customer deleted successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error deleting customer: {ex.Message}");
            }
        }
        [HttpGet("GetDashboard/{CustomerId}")]
        
        public async Task<IActionResult> GetDashboard(int CustomerId)
        {
            try
            {
                var dashboard = await _customerService.GetCustomerDashboardAsync(CustomerId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving dashboard: {ex.Message}" });
            }
        }
    }
}
