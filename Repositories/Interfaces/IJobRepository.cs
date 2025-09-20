using CommunityAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<ActionResults> CreateJobAsync(Job job);
        Task<ActionResults> CreateJobQuotationAsync(JobQuotationRequest job);
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId);
        Task<JobMedia> GetMediaByIdAsync(long uid);

        Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId);
        Task<IEnumerable<QuotationRequestWithResponseDto>> GetAllQuotationrequestsByVendor(long VendorId);

        Task<IEnumerable<Job>> GetCustomerJobsByJobIdAsync(long JobId);

        Task<ActionResults> InsertJobQuotationResponseAsync(JobQuotationResponse response);

        Task<ActionResults> InsertJobQuotationResponseAcceptAsync(JobQuotationResponseAccept response);

        Task<List<CustomerJobWithEmployeesDto>> GetvendorIdJobsWithEmployeesAsync(long vendorId);

        Task<List<CustomerJobWithEmployeesDto>> GetJobsWithEmployeesAsync(long customerId);

        Task<ActionResults> AssignEmployeesAsync(AssignEmployeeRequest request);

        Task<ActionResults> InsertJobCompletionWithPhotosAsync(JobCompletionRequestDto request);
        Task<IEnumerable<RequestedJobs>> GetAllRequestedJobs(long customerId);
        Task<QuotationResponseDto> GetQuotationResponseByIdAsync(long responseId);

        Task<JobInfoDetailDto> GetJobInfoDetailByJobIdAsync(long jobId);
        Task<JobCompletionResponseDto> GetJobCompletionDetailsAsync(long jobId);

        Task<ActionResults> UpdateJobCompletionByCustomerAsync(CustomerJobCompletionUpdateDto dto);
        Task<ActionResults> UpdateJobStatus(JobStatusUpdateDto dto);
        Task<JobStatusTrackingResponse> GetJobStatusTrackingAsync(long jobId);
        Task<IEnumerable<VendorHistoryListDto>> GetVendorHistoryListAsync(long vendorId);
        Task<HistoryDetailDto> GetVendorHistoryDetailAsync(long vendorId, long? jobId);
        Task<IEnumerable<CustomerHistoryListDto>> GetCustomerHistoryListAsync(long customerId);
        Task<CustomerHistoryDetailDto> GetCustomerHistoryDetailAsync(long customerId, long? jobId);
        Task<ServiceDetailDto> GetServiceDetailAsync(long vendorId, long serviceId);
    }

}
