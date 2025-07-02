using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<long> CreateJobAsync(Job job);
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId);
        Task<JobMedia> GetMediaByIdAsync(long uid);

        Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId);
    }

}
