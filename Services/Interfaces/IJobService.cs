using CommunityAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IJobService
    {
        Task<ActionResults> CreateJobAsync(Job job);
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId);

        Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId);
        Task<IEnumerable<Job>> GetCustomerJobsByJobIdAsync(long JobId);
    }

}
