using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IConfiguration _config;

        public JobRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        public async Task<long> CreateJobAsync(Job job)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", job.CustomerId);
            parameters.Add("@ServiceId", job.ServiceId);
            parameters.Add("@Remarks", job.Remarks);
            parameters.Add("@Active", job.Active);
            parameters.Add("@CreatedBy", job.CreatedBy);
            parameters.Add("@JobId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await CreateConnection().ExecuteAsync("usp_Jobs_Insert", parameters, commandType: CommandType.StoredProcedure);

            long jobId = parameters.Get<long>("@JobId");

            await CreateConnection().ExecuteAsync("usp_JobDistribution_InsertTopVendors",
                new { JobId = jobId, ServiceId = job.ServiceId, CreatedBy = job.CreatedBy },
                commandType: CommandType.StoredProcedure);

            return jobId;
        }

        public async Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(long customerId)
        {
            using var multi = await CreateConnection().QueryMultipleAsync("usp_GetJobsByCustomerId",
                new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);

            var jobs = await multi.ReadAsync<Job>();
            var media = await multi.ReadAsync<JobMedia>();
            var distribution = await multi.ReadAsync<JobDistribution>();

            foreach (var job in jobs)
            {
                job.MediaList = media.Where(m => m.JobId == job.JobId).ToList();
                job.Distributions = distribution.Where(d => d.JobId == job.JobId).ToList();
            }

            return jobs;
        }



        public async Task<JobMedia> GetMediaByIdAsync(long uid)
        {

            using var connection = CreateConnection();
            var jobs = await connection.QueryFirstOrDefaultAsync<JobMedia>(
                "usp_GetJobsByCustomerId",
                new { CustomerId = uid },
                commandType: CommandType.StoredProcedure
            );
            return jobs;
        }

        public async Task<IEnumerable<Job>> GetJobsByVendorIdAsync(long VendorId)
        {
            using var multi = await CreateConnection().QueryMultipleAsync("usp_GetJobsByVendorId",
                new { CustomerId = VendorId }, commandType: CommandType.StoredProcedure);

            var jobs = await multi.ReadAsync<Job>();
            var media = await multi.ReadAsync<JobMedia>();
            var distribution = await multi.ReadAsync<JobDistribution>();

            foreach (var job in jobs)
            {
                job.MediaList = media.Where(m => m.JobId == job.JobId).ToList();
                job.Distributions = distribution.Where(d => d.JobId == job.JobId).ToList();
            }

            return jobs;
        }

    }

}
