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
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IResponseService _responseService;

        public JobController(IJobService jobService, IResponseService responseService)
        {
            _jobService = jobService;
            _responseService = responseService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateJob([FromBody] Job job)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                var resultMessage = await _jobService.CreateJobAsync(job);
                if (!resultMessage.Success)
                    return _responseService.ConflictResponse(resultMessage.Message); // 409 Conflict    
                return _responseService.SuccessResponse(resultMessage.Id, "Job created and vendors assigned successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating job: {ex.Message}");
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetJobsByCustomer(long customerId)
        {
            try
            {
                var jobs = await _jobService.GetJobsByCustomerIdAsync(customerId);
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }
        }
        [HttpGet("JobsByVendor/{VendorId}")]
        public async Task<IActionResult> GetJobsByVendor(long VendorId)
        {
            try
            {
                var jobs = await _jobService.GetJobsByVendorIdAsync(VendorId);
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("GetCustomerJobsByJobId/{JobId}")]
        public async Task<IActionResult> GetCustomerJobsByJobIdAsync(long JobId)
        {
            try
            {
                var jobs = await _jobService.GetCustomerJobsByJobIdAsync(JobId);
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }
        }
    }

}
