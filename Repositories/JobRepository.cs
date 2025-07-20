using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Transactions;

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
        public async Task<ActionResults> CreateJobAsync(Job job)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CustomerId", job.CustomerId);
                parameters.Add("@ServiceId", job.ServiceId);
                parameters.Add("@Remarks", job.Remarks);
                parameters.Add("@Active", true);
                parameters.Add("@CreatedBy", job.CreatedBy);
                parameters.Add("@ExpectedDate", job.ExpectedDate);
                parameters.Add("@ContactNumber", job.ContactNumber);
                parameters.Add("@Priority", job.Priority);
                parameters.Add("@JobId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                // ✅ Pass transaction here
                await connection.ExecuteAsync("usp_Jobs_Create", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                long jobId = parameters.Get<long>("@JobId");

                string json = JsonConvert.SerializeObject(job.MediaList);

                var parameters2 = new DynamicParameters();
                parameters2.Add("@mediaListJson", json, DbType.String);
                parameters2.Add("@CustomerId", job.CustomerId);
                parameters2.Add("@JobId", jobId);
                parameters2.Add("@CreatedBy", job.CreatedBy);

                // ✅ Pass transaction here
                await connection.ExecuteAsync("usp_InsertJobMediaList", parameters2, commandType: CommandType.StoredProcedure, transaction: transaction);

                // ✅ Pass transaction here
                //await connection.ExecuteAsync("usp_JobDistribution_InsertTopVendors",
                //    new { JobId = jobId, ServiceId = job.ServiceId, CreatedBy = job.CreatedBy },
                //    commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return new ActionResults
                {
                    Success = true,
                    Id = jobId,
                    Message = "Job Listing registered successfully."
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var errorId = Guid.NewGuid();
                var timestamp = DateTime.UtcNow.ToString("u");
                return new ActionResults
                {
                    Success = false,
                    Id = 0,
                    Message = $"Error: {ex.Message} | Reference ID: {errorId} | Timestamp: {timestamp}"
                };
            }
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

        public async Task<IEnumerable<Job>> GetCustomerJobsByJobIdAsync(long JobId)
        {
            using var multi = await CreateConnection().QueryMultipleAsync("usp_GetCustomerJobsByJobId",
                new { jobId = JobId }, commandType: CommandType.StoredProcedure);

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
