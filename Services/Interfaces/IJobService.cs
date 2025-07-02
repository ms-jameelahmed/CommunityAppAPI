using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IJobService
    {
        Task<long> CreateJobAsync(Job job);
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId);

        Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId);
    }

}
