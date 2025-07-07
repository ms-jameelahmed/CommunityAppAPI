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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterVendor([FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                var result = await _vendorService.RegisterVendorAsync(vendor);
                if (!string.IsNullOrEmpty(result))
                    return _responseService.ConflictResponse(result); // 409 Conflict
                
                    return _responseService.SuccessResponse(vendor, "Vendor registered successfully");

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error registering vendor: {ex.Message}");
            }
        }

        [HttpGet("get/{id:long}")]
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

        [HttpGet("getall")]
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

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] Vendor vendor)
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

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] string modifiedBy)
        {
            try
            {
                var result = await _vendorService.DeleteVendorAsync(id, modifiedBy);
                if (result)
                    return _responseService.SuccessResponse(null, "Vendor deleted successfully");

                return _responseService.ErrorResponse("Vendor deletion failed");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error deleting vendor: {ex.Message}");
            }
        }
    }
}

