using Azure.Core;
using CommunityAppAPI.Models;
using CommunityAppAPI.Services.Common;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteVisitController : ControllerBase
    {
        private readonly ISiteVisitService _siteVisitService;
        private readonly IResponseService _responseService;

        public SiteVisitController(ISiteVisitService siteVisitService, IResponseService responseService)
        {
            _siteVisitService = siteVisitService;
            _responseService = responseService;
        }

        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateSiteVisitRequest([FromBody] SiteVisitRequestDto request)
        {
            try
            {
                var id = await _siteVisitService.CreateSiteVisitRequestAsync(request);
                return _responseService.SuccessResponse(id, "Site Visit request created successfully.");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating site visit: {ex.Message}");
            }
        }

        [HttpPost("AssignEmployee")]
        public async Task<IActionResult> AssignEmployee(AssignSiteVisitEmployeeRequest request)
        {
            try
            {
                var success = await _siteVisitService.AssignEmployeesAsync(request);
                if (success.Success)
                    return _responseService.SuccessResponse(null, "Employee assigned successfully");

                return _responseService.ErrorResponse("Employee assign to site vist failed");
                
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error assigning employee: {ex.Message}");
            }
        }

        [HttpGet("Detail/{jobid}")]
        public async Task<IActionResult> GetSiteVisitDetail(int jobid)
        {
            try
            {
                var detail = await _siteVisitService.GetSiteVisitDetailAsync(jobid);
                return _responseService.SuccessResponse(detail, "Site Visit detail fetched successfully.");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching detail: {ex.Message}");
            }
        }

        [HttpPost("CustomerResponse")]
        public async Task<IActionResult> UpdateCustomerResponse(CustomerResponseDto responseDto)
        {
            try
            {
                var result = await _siteVisitService.UpdateCustomerResponseAsync(responseDto.siteVisitId, responseDto.isAccepted);

                if (!result.Success)
                    return _responseService.ErrorResponse("Failed to update response.");
                if (result.Success)
                {
                    if (responseDto.isAccepted)
                    {
                        GatePassRequest getjobpass = await _siteVisitService.GetGatePassRequestByIdAsync(responseDto.jobid, responseDto.siteVisitId);
                        var emailContent = await _siteVisitService.BuildEmailContentAsync(getjobpass);

                        return Ok(new
                        {
                            Message = "Response updated successfully.",
                            EmailPreview = emailContent
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Message = "Response updated successfully."
                        });
                    }
                }
                return _responseService.ErrorResponse("Failed to update response.");

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error updating response: {ex.Message}");
            }
        }

        [HttpGet("ValidateQRCode")]
        public async Task<IActionResult> ValidateQRCode(string qrCode)
        {
            try
            {
                var isValid = await _siteVisitService.ValidateQRCodeAsync(qrCode);
                return _responseService.SuccessResponse(isValid, isValid ? "Valid QR Code." : "Invalid QR Code.");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error validating QR Code: {ex.Message}");
            }
        }
    }
    
}
