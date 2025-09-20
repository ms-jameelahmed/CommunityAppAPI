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
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly IResponseService _responseService;

        public VendorController(IVendorService vendorService, IResponseService responseService)
        {
            _vendorService = vendorService;
            _responseService = responseService;
        }

        [HttpPost("RegisterVendor")]
        
        public async Task<IActionResult> RegisterVendor([FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                var result = await _vendorService.RegisterVendorAsync(vendor);
                if (!result.Success)
                    return _responseService.ConflictResponse(result.Message);



                return _responseService.SuccessResponse(vendor, "Vendor registered successfully");

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error registering vendor: {ex.Message}");
            }
        }
        
        [HttpGet("getVendorbyid/{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var vendor = await _vendorService.GetByIdAsync(id);
                if (vendor == null)
                    return _responseService.NotFoundResponse("Vendor not found");

                return _responseService.SuccessResponse(vendor, "Vendor fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error retrieving vendor: {ex.Message}");
            }
        }
        
        [HttpGet("GetAllVendors")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vendors = await _vendorService.GetAllAsync();
                return _responseService.SuccessResponse(vendors, "Vendor list fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error retrieving vendors: {ex.Message}");
            }
        }
        
        [HttpPut("updateVendors/{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateCustomer vendor)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                vendor.CustomerId = id;
                vendor.ModifiedDate = DateTime.Now;
                var result = await _vendorService.UpdateVendorAsync(vendor);
                if (result!="")
                    return _responseService.SuccessResponse(vendor, "Vendor updated successfully");

                return _responseService.ErrorResponse("Vendor update failed");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error updating vendor: {ex.Message}");
            }
        }
        
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteCustomer customer)
        {
            try
            {
                var result = await _vendorService.DeleteVendorAsync(customer.CustomerId, customer.ModifiedBy);
                if (result)
                    return _responseService.SuccessResponse(null, "Vendor deleted successfully");

                return _responseService.ErrorResponse("Vendor deletion failed");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error deleting vendor: {ex.Message}");
            }
        }
        
        [HttpGet("GetAllVendorServices/{vendorId:long}")]
        public async Task<IActionResult> GetAllVendorServices(long vendorId)
        {
            var result = await _vendorService.GetVendorServicesAsync(vendorId);
            return Ok(result);
        }
        
        [HttpPost("AddVendorService")]
        public async Task<IActionResult> AddVendorService([FromBody] Vendor_Service vendorService)
        {

            try
            {
                var result = await _vendorService.InsertVendorServiceAsync(vendorService);
                if (result.Success)
                    return _responseService.SuccessResponse(result, "Inserted successfully");

                return _responseService.ErrorResponse("Vendor deletion failed");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error.. Insert failed: {ex.Message}");
            }
           
        }
        
        [HttpPost("updateVendorService")]
        public async Task<IActionResult> updateVendorService([FromBody] Vendor_Service vendorService)
        {
            try
            {
                var result = await _vendorService.UpdateVendorServiceAsync(vendorService);
                if (result.Success)
                    return _responseService.SuccessResponse(result, "Updated successfully");

                return _responseService.ErrorResponse("Vendor service Update failed");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error.. Update failed: {ex.Message}");
            }
             
        }

        [HttpGet("GetAllVendorsforService/{serviceId:long}")]
        
        public async Task<IActionResult> GetAllVendorsforService(long serviceId)
        {
            var result = await _vendorService.GetAllVendorsforService(serviceId);
            return Ok(result);
        }

        [HttpGet("GetVendorDashboard/{vendorId}")]
        
        public async Task<IActionResult> GetDashboard(int vendorId)
        {
            try
            {
                var dashboard = await _vendorService.GetVendorDashboardAsync(vendorId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving dashboard: {ex.Message}" });
            }
        }
    }
}

