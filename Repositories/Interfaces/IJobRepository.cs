using CommunityAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<ActionResults> CreateJobAsync(Job job);
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId);
        Task<JobMedia> GetMediaByIdAsync(long uid);

        Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId);

        Task<IEnumerable<Job>> GetCustomerJobsByJobIdAsync(long JobId);
    }

}
