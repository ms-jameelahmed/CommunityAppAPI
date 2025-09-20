using Azure;
using Azure.Core;
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
        private readonly ICommonService _commonService;
        private readonly IResponseService _responseService;

        public JobController(IJobService jobService, IResponseService responseService, ICommonService commonService)
        {
            _jobService = jobService;
            _responseService = responseService;
            _commonService = commonService;
        }

        
        [HttpPost("CreateJob")]
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
        
        [HttpPost("CreateJobQuotationRequest")]
        public async Task<IActionResult> CreateJobQuotationRequest([FromBody] JobQuotationRequest job)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                var resultMessage = await _jobService.CreateJobQuotationAsync(job);
                if (!resultMessage.Success)
                    return _responseService.ConflictResponse(resultMessage.Message); // 409 Conflict    
                _commonService.SendMultpleNotificationByJobAsync(job.JobId, "Job Quotation Request", "Job Quotation Requested with JobId: "+ job.JobId + ".", "1");
                return _responseService.SuccessResponse(resultMessage.Id, " Job Quotation assigned to vendors  successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating job: {ex.Message}");
            }
        }
        [HttpGet("GetJobsByCustomer/{customerId}")]
        
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
        [HttpGet("GetAllQuotationsByVendor/{VendorId}")]
        
        public async Task<IActionResult> GetAllQuotationrequestsByVendor(long VendorId)
        {
            try
            {
                var jobs = await _jobService.GetAllQuotationrequestsByVendor(VendorId);
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }
        }
        
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

        
        [HttpPost("CreateJobQuotationResponse")]
        
        public async Task<IActionResult> Post([FromBody] JobQuotationResponse response)
        {

            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                var resultMessage = await _jobService.AddJobQuotationResponseAsync(response);
                if (!resultMessage.Success)
                    return _responseService.ConflictResponse(resultMessage.Message); // 409 Conflict
                                                                                     // 
                _commonService.SendMultpleNotificationByJobAsync(resultMessage.Id, "Job Quotation Response", "Update from Vendor.. Job Quotation Response on JobId: " + response.JobId + ".", "2");

                return _responseService.SuccessResponse(resultMessage.Id, resultMessage.Message);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating job Quotation response : {ex.Message}");
            }
            
        }


        [HttpPost("CreateJobBooking")]
        
        public async Task<IActionResult> CreateJobBooking([FromBody] JobQuotationResponseAccept response)
        {

            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);

            try
            {
                //var resultMessage = await _jobService.InsertJobQuotationResponseAcceptAsync(response);
                //if (!resultMessage.Success)
                //    return _responseService.ConflictResponse(resultMessage.Message); // 409 Conflict
                _commonService.SendMultpleNotificationByJobAsync(response.JobId, "Job Quotation Accepted", "Update from Customer.. Job Quotation Accepted on JobId: " + response.JobId + ".", "4");

                return _responseService.SuccessResponse("", "");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating job Quotation response : {ex.Message}");
            }

        }
        
        [HttpGet("GetVendorJobs/{vendorId}")]
        public async Task<IActionResult> GetVendorJobs(long vendorId)
        {
            try
            {
                var jobs = await _jobService.GetJobsByVendorIdAsync(vendorId);
                if (jobs == null)
                    return _responseService.ErrorResponse($"No job details found for the vendor.");  
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }
            
            
        }

        [HttpGet("GetVendorOngoingJobs/{vendorId}")]
        
        public async Task<IActionResult> GetVendorOngoingJobs(long vendorId)
        {
            try
            {
                var jobs = await _jobService.GetvendorIdJobsWithEmployeesAsync(vendorId);
                if (jobs == null || jobs.Count == 0)
                    return _responseService.ErrorResponse($"No job details found for the vendor.");
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }


        }
        
        [HttpGet("GetCustomerOngoingJobs/{customerId}")]
        public async Task<IActionResult> GetCustomerOngoingJobs(long customerId)
        {
            try
            {
                var jobs = await _jobService.GetongoingJobsbyCustomerId(customerId);
                if (jobs == null || jobs.Count == 0)
                    return _responseService.ErrorResponse($"No job details found for the vendor.");
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }


        }

        [HttpPost("AssignEmployees")]
        
        public async Task<IActionResult> AssignEmployees([FromBody] AssignEmployeeRequest request)
        {
            if (request.JobId <= 0 || request.CustomerId <= 0 || request.AssignEmployeeList == null || !request.AssignEmployeeList.Any())
                return _responseService.ErrorResponse("Invalid request data.");

            try
            {
                var resultMessage = await _jobService.AssignEmployeesAsync(request);
                if (!resultMessage.Success)
                    return _responseService.ConflictResponse(resultMessage.Message); // 409 Conflict    
                _commonService.SendMultpleNotificationByJobAsync(resultMessage.Id, "Job Update", "Job Update on JobId: " + request.JobId + "..Employees Assigned..", "5");

                return _responseService.SuccessResponse(resultMessage.Id, resultMessage.Message);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error creating Employees assigning. : {ex.Message}");
            }


          
        }

        [HttpPost("JobCompletion")]
        
        public async Task<IActionResult> SubmitJobCompletion(JobCompletionRequestDto request)
        {
            if (!ModelState.IsValid)
                return _responseService.ValidationErrorResponse(ModelState);
            try
            { 
                var resultMessage = await _jobService.InsertJobCompletionWithPhotosAsync(request);
                if (!resultMessage.Success)
                    return _responseService.ConflictResponse("Failed to save job completion."); // 409 Conflict
                                                                                                // 
                _commonService.SendMultpleNotificationByJobAsync(resultMessage.Id, "Job Update", "Job Update JobId: " + request.JobId + "..Job completion submitted..", "5");

                return _responseService.SuccessResponse(null, "Job completion submitted.");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet("GetAllRequestedJobs/{customerId}")]
        
        public async Task<IActionResult> GetAllRequestedJobs(long customerId)
        {
            try
            {
                var jobs = await _jobService.GetAllRequestedJobs(customerId);
                return _responseService.SuccessResponse(jobs, "Jobs fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching jobs: {ex.Message}");
            }
        }

        [HttpGet("GetquotationResponseDetails/{quotationResponseId}")]
        
        public async Task<IActionResult> GetResponseDetails(long quotationResponseId)
        {
            var result = await _jobService.GetResponseDetailsAsync(quotationResponseId);
            if (result == null)
                return NotFound("Quotation Response not found");

            return Ok(result);
        }

        [HttpGet("JobInfoDetail/{jobId}")]
        
        public async Task<IActionResult> GetJobInfoDetail(long jobId)
        {
            var result = await _jobService.GetJobInfoDetailAsync(jobId);
            if (result == null)
                return NotFound(new { Message = "Job not found" });

            return Ok(result);
        }

        [HttpGet("GetJobCompletionDetails/{jobId}")]
        
        public async Task<IActionResult> GetJobCompletionDetails(long jobId)
        {
            var result = await _jobService.GetJobCompletionDetailsAsync(jobId);
            if (result == null || !result.Photos.Any())
                return NotFound("No completion details found for this JobId.");

            return Ok(result);
        }

        [HttpPost("update-customer-completion")]
        
        public async Task<IActionResult> UpdateJobCompletionByCustomer([FromBody] CustomerJobCompletionUpdateDto dto)
        {
            var resultMessage = await _jobService.UpdateJobCompletionByCustomerAsync(dto);
            if (!resultMessage.Success)
                return _responseService.ConflictResponse("Failed to save job completion."); // 409 Conflict    
            _commonService.SendMultpleNotificationByJobAsync(resultMessage.Id, "Job Update", "Job Update JobId: " + dto.JobId + "..Job completion updated successfully by customer..", "4");

            return _responseService.SuccessResponse(null, "Job completion updated successfully by customer.");
            
        }

        [HttpPost("UpdateJobStatus")]
        
        public async Task<IActionResult> UpdateJobStatus([FromBody] JobStatusUpdateDto dto)
        {
            var resultMessage = await _jobService.UpdateJobStatus(dto);
            if (!resultMessage.Success)
                return _responseService.ConflictResponse("Failed to update job status."); // 409 Conflict    
            _commonService.SendMultpleNotificationByJobAsync(dto.JobId, "Job Update", "Job Update JobId: " + dto.JobId + "..Job status updated..", "5");

            return _responseService.SuccessResponse(null, "Job status updated successfully.");

        }
        [HttpPost("JobStatusTracking")]
        
        public async Task<IActionResult> GetJobStatusTracking(JobStatusTracking JobStatusTracking)
        {
            var result = await _jobService.GetJobStatusTrackingAsync(JobStatusTracking.JobId);
            return _responseService.SuccessResponse(result, "Job status tracking retrieved successfully");
        }
        
        [HttpPost("VendorHistoryList")]
        public async Task<IActionResult> GetVendorHistoryList(JobVendorHistoryTracking vendorHistoryTracking)
        {
            try
            {
                var jobs = await _jobService.GetVendorHistoryListAsync(vendorHistoryTracking.VendorId);
                return _responseService.SuccessResponse(jobs, "Vendor History fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Vendor History: {ex.Message}");
            }
            
        }

        
        [HttpPost("VendorHistoryDetail")]
        public async Task<IActionResult> GetVendorHistoryDetail(JobVendorHistoryTracking vendorHistoryTracking)
        {
            try
            {
                var jobs = await _jobService.GetVendorHistoryDetailAsync(vendorHistoryTracking.VendorId, vendorHistoryTracking.JobId);
                return _responseService.SuccessResponse(jobs, "Vendor History Detail fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Vendor History Detail not found: {ex.Message}");
            }
             
        }

        
        [HttpPost("CustomerHistoryList")]
        public async Task<IActionResult> GetCustomerHistoryList(JobCustomerHistoryTracking customerHistoryTracking)
        {
            try
            {
                var jobs = await _jobService.GetCustomerHistoryListAsync(customerHistoryTracking.CustomerId);
                return _responseService.SuccessResponse(jobs, "Customer History fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Customer History: {ex.Message}");
            }
        }

        
        [HttpPost("CustomerHistoryDetail")]
        public async Task<IActionResult> GetCustomerHistoryDetail(JobCustomerHistoryTracking customerHistoryTracking)
        {
            try
            {
                var jobDetail = await _jobService.GetCustomerHistoryDetailAsync(
                    customerHistoryTracking.CustomerId,
                    customerHistoryTracking.JobId);

                return _responseService.SuccessResponse(jobDetail, "Customer History Detail fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Customer History Detail not found: {ex.Message}");
            }
        }

        
        [HttpPost("ServiceDetail")]
        public async Task<IActionResult> GetServiceDetail(JobServiceDetail jobServiceDetail)
        {
            try
            {
                var result = await _jobService.GetServiceDetailAsync(jobServiceDetail.VendorId, jobServiceDetail.ServiceId);
                return _responseService.SuccessResponse(result, "Service details fetched successfully");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching service detail: {ex.Message}");
            }
        }
    }

}
