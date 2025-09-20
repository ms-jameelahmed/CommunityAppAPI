using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<ActionResults> CreateJobAsync(Job job)
        {
            return await _jobRepository.CreateJobAsync(job);
        }
        public async Task<ActionResults> CreateJobQuotationAsync(JobQuotationRequest job)
        {
            return await _jobRepository.CreateJobQuotationAsync(job);
        }
        public async Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId)
        {
            return await _jobRepository.GetJobsByCustomerIdAsync(customerId);
        }

        public async Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId)
        {
            return await _jobRepository.GetJobsByVendorIdAsync(VendorId);
        }
        public async Task<IEnumerable<QuotationRequestWithResponseDto>> GetAllQuotationrequestsByVendor(long VendorId)
        {
            return await _jobRepository.GetAllQuotationrequestsByVendor(VendorId);
        }
        public async Task<IEnumerable<Job>> GetCustomerJobsByJobIdAsync(long JobId)
        {
            return await _jobRepository.GetCustomerJobsByJobIdAsync(JobId);
        }

        public async Task<ActionResults> AddJobQuotationResponseAsync(JobQuotationResponse response)
        {
            return await _jobRepository.InsertJobQuotationResponseAsync(response);
        }

        public async Task<ActionResults> InsertJobQuotationResponseAcceptAsync(JobQuotationResponseAccept response)
        {
            return await _jobRepository.InsertJobQuotationResponseAcceptAsync( response);
        }

        public async Task<List<CustomerJobWithEmployeesDto>> GetvendorIdJobsWithEmployeesAsync(long vendorId)
        {
            return await _jobRepository.GetvendorIdJobsWithEmployeesAsync(vendorId);
        }
        public async Task<List<CustomerJobWithEmployeesDto>> GetongoingJobsbyCustomerId(long customerId)
        {
            return await _jobRepository.GetJobsWithEmployeesAsync(customerId);
        }

        public async Task<ActionResults> AssignEmployeesAsync(AssignEmployeeRequest request)
        {
            return await _jobRepository.AssignEmployeesAsync(request);
        }

        public async Task<ActionResults> InsertJobCompletionWithPhotosAsync(JobCompletionRequestDto request)
        {
            return await _jobRepository.InsertJobCompletionWithPhotosAsync(request);
        }
        public async Task<IEnumerable<RequestedJobs>> GetAllRequestedJobs(long customerId)
        {
            return await _jobRepository.GetAllRequestedJobs(customerId);
        }
        public async Task<QuotationResponseDto> GetResponseDetailsAsync(long responseId)
        {
            return await _jobRepository.GetQuotationResponseByIdAsync(responseId);
        }

        public async Task<JobInfoDetailDto> GetJobInfoDetailAsync(long jobId)
        {
            return await _jobRepository.GetJobInfoDetailByJobIdAsync(jobId);
        }

        public async Task<JobCompletionResponseDto> GetJobCompletionDetailsAsync(long jobId)
        {
            return await _jobRepository.GetJobCompletionDetailsAsync(jobId);
        }

        public async Task<ActionResults> UpdateJobCompletionByCustomerAsync(CustomerJobCompletionUpdateDto dto)
        {
            return await _jobRepository.UpdateJobCompletionByCustomerAsync(dto);
        }

        public async Task<ActionResults> UpdateJobStatus(JobStatusUpdateDto dto)
        {
            return await _jobRepository.UpdateJobStatus(dto);
        }
        public async Task<JobStatusTrackingResponse> GetJobStatusTrackingAsync(long jobId)
        {
            return await _jobRepository.GetJobStatusTrackingAsync(jobId);
        }
        public async Task<IEnumerable<VendorHistoryListDto>> GetVendorHistoryListAsync(long vendorId)
        {
            return await _jobRepository.GetVendorHistoryListAsync(vendorId);
        }

        public async Task<HistoryDetailDto> GetVendorHistoryDetailAsync(long vendorId, long? jobId)
        {
            return await _jobRepository.GetVendorHistoryDetailAsync(vendorId, jobId);
        }
        public async Task<IEnumerable<CustomerHistoryListDto>> GetCustomerHistoryListAsync(long customerId)
        {
            return await _jobRepository.GetCustomerHistoryListAsync(customerId);
        }

        public async Task<CustomerHistoryDetailDto> GetCustomerHistoryDetailAsync(long customerId, long? jobId)
        {
            return await _jobRepository.GetCustomerHistoryDetailAsync(customerId, jobId);
        }
        public async Task<ServiceDetailDto> GetServiceDetailAsync(long vendorId, long serviceId)
        {
            return await _jobRepository.GetServiceDetailAsync(vendorId,serviceId);
        }
       
    }

}
