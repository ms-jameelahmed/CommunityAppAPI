using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;

namespace CommunityAppAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<long> CreateJobAsync(Job job)
        {
            return await _jobRepository.CreateJobAsync(job);
        }

        public async Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId)
        {
            return await _jobRepository.GetJobsByCustomerIdAsync(customerId);
        }

        public async Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId)
        {
            return await _jobRepository.GetJobsByVendorIdAsync(VendorId);
        }
    }

}
