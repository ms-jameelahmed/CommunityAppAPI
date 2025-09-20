using Azure.Core;
using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Connections;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
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
                parameters.Add("@SiteVisitRequired", job.SiteVisitRequired);
                parameters.Add("@JobId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                // ✅ Pass transaction here
                await connection.ExecuteAsync("usp_Jobs_Create", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                long jobId = parameters.Get<long>("@JobId");

                string json = JsonConvert.SerializeObject(job.MediaList);

                foreach (var formFile in job.MediaList)
                {


                    var parameters2 = new DynamicParameters();

                    parameters2.Add("@CustomerId", job.CustomerId);
                    parameters2.Add("@JobId", jobId);
                    parameters2.Add("@UploadFrom", "C");
                    parameters2.Add("@CreatedBy", job.CreatedBy);

                    parameters2.Add("@PhotoVideoType", formFile.PhotoVideoType);
                    parameters2.Add("@FileContent", formFile.FileContent);
                    parameters2.Add("@Type", formFile.Type);
                    parameters2.Add("@InRefUID", formFile.InRefUID);
                    parameters2.Add("@UID", formFile.UID);

                    // ✅ Pass transaction here
                    await connection.ExecuteAsync("usp_InsertJobMediaList", parameters2, commandType: CommandType.StoredProcedure, transaction: transaction);


                }



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
        public async Task<ActionResults> CreateJobQuotationAsync(JobQuotationRequest job)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {

                foreach (var jobQoutationRequest in job.JobQuotationRequestItems)
                {
                    await connection.ExecuteAsync("usp_JobDistribution_InsertTopVendors",
                    new
                    {
                        jobId = job.JobId,
                        serviceId = job.ServiceId,
                        customerId = job.FromCustomerId,
                        venodrid = jobQoutationRequest.VendorId,
                        createdBy = job.CreatedBy
                    },
                    commandType: CommandType.StoredProcedure, transaction: transaction);

                }
                transaction.Commit();

                return new ActionResults
                {
                    Success = true,
                    Id = null,
                    Message = "Job Distribution registered successfully."
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
            var jobQuotationRequests = (await multi.ReadAsync<JobQuotationResponse>()).ToList();
            var quotationItems = (await multi.ReadAsync<JobQuotationResponseItems>()).ToList();

            foreach (var job in jobs)
            {
                job.MediaList = media.Where(m => m.JobId == job.JobId).ToList();

                var jobRequests = jobQuotationRequests
                    .Where(qr => qr.JobId == job.JobId)
                    .ToList();

                foreach (var req in jobRequests)
                {
                    req.JobQuotationResponseItems = quotationItems
                        .Where(qi => qi.QuotationResponceId == req.QuotationResponceId)
                        .ToList();
                }

                job.JobQuotationResponce = jobRequests;
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

        public async Task<IEnumerable<QuotationRequestWithResponseDto>> GetAllQuotationrequestsByVendor(long VendorId)
        {
            using var multi = await CreateConnection().QueryMultipleAsync("usp_GetQuotationRequestByVendorId",
                new { CustomerId = VendorId }, commandType: CommandType.StoredProcedure);


            var Quotationrequests = await multi.ReadAsync<QuotationRequestWithResponseDto>();



            return Quotationrequests;
        }

        public async Task<ActionResults> InsertJobQuotationResponseAsync(JobQuotationResponse response)
        {
            try
            {

                var connection = (SqlConnection)CreateConnection();
                await connection.OpenAsync();
                var itemsTable = new DataTable();
                itemsTable.Columns.Add("Product", typeof(string));
                itemsTable.Columns.Add("Quantity", typeof(int));
                itemsTable.Columns.Add("Price", typeof(decimal));
                itemsTable.Columns.Add("TotalAmount", typeof(decimal));

                foreach (var item in response.JobQuotationResponseItems)
                {
                    itemsTable.Rows.Add(item.Product, item.Quantity, item.Price, item.TotalAmount);
                }

                var parameters = new DynamicParameters();
                parameters.Add("@JobId", response.JobId);
                parameters.Add("@QuotationRequestId", response.QuotationRequestId);
                parameters.Add("@ServiceId", response.ServiceId);
                parameters.Add("@VendorId", response.VendorId);
                parameters.Add("@QuotationDetails", response.QuotationDetails);
                parameters.Add("@QuotationAmount", response.QuotationAmount);
                parameters.Add("@ServiceCharge", response.ServiceCharge);
                parameters.Add("@StartDate", response.StartDate);
                parameters.Add("@EndDate", response.EndDate);
                parameters.Add("@CreatedBy", response.CreatedBy);
                parameters.Add("@Status", response.Status);
                parameters.Add("@JobQuotationItems", itemsTable.AsTableValuedParameter("JobQuotationItemType"));

                await connection.ExecuteAsync("usp_InsertJobQuotationResponse", parameters, commandType: CommandType.StoredProcedure);
                connection.Close();
                return new ActionResults
                {
                    Success = true,
                    Id = null,
                    Message = "Job  Quotation Response registered successfully."
                };
            }
            catch (Exception ex)
            {

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
        public async Task<ActionResults> InsertJobQuotationResponseAcceptAsync(JobQuotationResponseAccept response)
        {
            try
            {

                var connection = (SqlConnection)CreateConnection();
                await connection.OpenAsync();


                var parameters = new DynamicParameters();
                parameters.Add("@JobId", response.JobId);
                parameters.Add("@QuotationRequestId", response.QuotationRequestId);
                parameters.Add("@QuotationResponseId", response.QuotationResponseId);

                parameters.Add("@VendorId", response.VendorId);

                parameters.Add("@CreatedBy", response.CreatedBy);
                parameters.Add("@Remarks", response.Remarks);

                await connection.ExecuteAsync("usp_InsertJobQuotationResponseAccept", parameters, commandType: CommandType.StoredProcedure);
                connection.Close();
                return new ActionResults
                {
                    Success = true,
                    Id = null,
                    Message = "Job  Quotation Response registered successfully."
                };
            }
            catch (Exception ex)
            {

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



        public async Task<List<CustomerJobWithEmployeesDto>> GetvendorIdJobsWithEmployeesAsync(long vendorId)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            var jobDictionary = new Dictionary<long, CustomerJobWithEmployeesDto>();

            var results = await connection.QueryAsync<CustomerJobWithEmployeesDto, AssignedEmployeeDto, CustomerJobWithEmployeesDto>(
                "usp_GetongoingJobsbyVendorId",
                (job, employee) =>
                {
                    if (!jobDictionary.TryGetValue(job.JobId, out var jobEntry))
                    {
                        jobEntry = job;
                        jobEntry.AssignedEmployees = new List<AssignedEmployeeDto>();
                        jobDictionary.Add(job.JobId, jobEntry);
                    }

                    if (!string.IsNullOrEmpty(employee?.EmployeeName))
                    {
                        jobEntry.AssignedEmployees.Add(employee);
                    }

                    return jobEntry;
                },
                new { vendorId = vendorId },
                splitOn: "EmployeeName",
                commandType: CommandType.StoredProcedure
            );

            return jobDictionary.Values.ToList();
        }
        public async Task<List<CustomerJobWithEmployeesDto>> GetJobsWithEmployeesAsync(long customerId)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            var jobDictionary = new Dictionary<long, CustomerJobWithEmployeesDto>();

            var results = await connection.QueryAsync<CustomerJobWithEmployeesDto, AssignedEmployeeDto, CustomerJobWithEmployeesDto>(
    "usp_GetOngoingJobsByCustomerId",
    (job, employee) =>
    {
        if (!jobDictionary.TryGetValue(job.JobId, out var jobEntry))
        {
            jobEntry = job;
            jobEntry.AssignedEmployees = new List<AssignedEmployeeDto>();
            jobDictionary.Add(job.JobId, jobEntry);
        }

        if (employee != null && !string.IsNullOrEmpty(employee.EmployeeName))
        {
            jobEntry.AssignedEmployees.Add(employee);
        }

        return jobEntry;
    },
    new { CustomerId = customerId },
    splitOn: "EmployeeName",
    commandType: CommandType.StoredProcedure
);

            var finalResult = jobDictionary.Values.ToList();
            return finalResult;
        }

        public async Task<ActionResults> AssignEmployeesAsync(AssignEmployeeRequest request)
        {
            try
            {
                var connection = (SqlConnection)CreateConnection();
                await connection.OpenAsync();
                var employeeTable = new DataTable();
                employeeTable.Columns.Add("EmployeeName", typeof(string));
                employeeTable.Columns.Add("EmployeePhoneNumber", typeof(string));
                employeeTable.Columns.Add("EmiratesId", typeof(string));
                employeeTable.Columns.Add("EmiratesIdPhoto", typeof(byte[])); // must match VARBINARY(MAX)
                employeeTable.Columns.Add("Email", typeof(string));
                foreach (var item in request.AssignEmployeeList)
                {
                    byte[]? photoBytes = null;

                    if (!string.IsNullOrWhiteSpace(item.EmiratesIdPhoto))
                    {
                        try
                        {
                            photoBytes = Convert.FromBase64String(item.EmiratesIdPhoto);
                        }
                        catch (FormatException)
                        {
                            // Handle base64 decode failure gracefully (optional)
                            photoBytes = null;
                        }
                    }

                    employeeTable.Rows.Add(
                        item.EmployeeName,
                        item.EmployeePhoneNumber,
                        item.EmiratesIdNumber,
                        photoBytes
                    );
                }

                var parameters = new DynamicParameters();

                using (var command = new SqlCommand("usp_InsertAssignedEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var tvpParam = new SqlParameter("@AssignEmployeeList", SqlDbType.Structured)
                    {
                        TypeName = "dbo.JobEmployee_Assignment", // exact type name in SQL Server
                        Value = employeeTable
                    };

                    command.Parameters.Add(tvpParam);
                    command.Parameters.AddWithValue("@JobId", request.JobId);
                    command.Parameters.AddWithValue("@CustomerId", request.CustomerId);


                    await command.ExecuteNonQueryAsync();
                    connection.Close();
                }


                return new ActionResults
                {
                    Success = true,
                    Id = null,
                    Message = "Employees assigned successfully."
                };
            }
            catch (Exception ex)
            {

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

        public async Task<ActionResults> InsertJobCompletionWithPhotosAsync(JobCompletionRequestDto request)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();

            var photoTable = new DataTable();
            photoTable.Columns.Add("BeforePhotoUrl", typeof(byte[]));
            photoTable.Columns.Add("AfterPhotoUrl", typeof(byte[]));

            foreach (var pair in request.PhotoPairs)
            {
                photoTable.Rows.Add(pair.BeforePhoto, pair.AfterPhoto);
            }

            var command = new SqlCommand("usp_InsertJobCompletionWithPhotos", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@JobId", request.JobId);
            command.Parameters.AddWithValue("@Notes", request.Notes ?? string.Empty);
            command.Parameters.AddWithValue("@CreatedBy", request.CreatedBy);

            var photoParam = new SqlParameter("@PhotoPairs", SqlDbType.Structured)
            {
                TypeName = "dbo.JobCompletionPhotoPairs",
                Value = photoTable
            };
            command.Parameters.Add(photoParam);

            await command.ExecuteNonQueryAsync();
            return new ActionResults
            {
                Success = true,
                Id = null,
                Message = "Insert Job Completion With Photos successfully."
            };
        }

        public async Task<IEnumerable<RequestedJobs>> GetAllRequestedJobs(long customerId)
        {
            using var multi = await CreateConnection().QueryMultipleAsync("usp_GetCustomerQuotationRequestsAwaiting",
                new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);


            var Quotationrequests = await multi.ReadAsync<RequestedJobs>();



            return Quotationrequests;
        }

        public async Task<QuotationResponseDto> GetQuotationResponseByIdAsync(long responseId)
        {
            using var multi = await CreateConnection().QueryMultipleAsync(
                "usp_GetQuotationResponseDetailsById",
                new { QuotationResponseId = responseId },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadFirstOrDefaultAsync<QuotationResponseDto>();
            if (response != null)
            {
                response.Items = (await multi.ReadAsync<QuotationItemDto>()).ToList();
            }

            return response;
        }

        public async Task<JobInfoDetailDto> GetJobInfoDetailByJobIdAsync(long jobId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", jobId);

            var result = await CreateConnection().QueryFirstOrDefaultAsync<JobInfoDetailDto>(
                "usp_GetJobInfoDetailByJobId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<JobCompletionResponseDto> GetJobCompletionDetailsAsync(long jobId)
        {
            var lookup = new JobCompletionResponseDto
            {
                Photos = new List<JobCompletionPhotoDto>()
            };

            await CreateConnection().QueryAsync<JobCompletionResponseDto, JobCompletionPhotoDto, JobCompletionResponseDto>(
                "usp_GetJobCompletionDetailsByJobId",
                (notes, photo) =>
                {
                    lookup.Notes = notes.Notes;
                    lookup.Photos.Add(photo);
                    return lookup;
                },
                new { JobId = jobId },
                splitOn: "BeforePhotoUrl",
                commandType: CommandType.StoredProcedure
            );

            return lookup;
        }

        public async Task<ActionResults> UpdateJobCompletionByCustomerAsync(CustomerJobCompletionUpdateDto dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", dto.JobId);
                parameters.Add("@Notes", dto.Notes);
                parameters.Add("@WorkDonePercentage", dto.WorkDonePercentage);
                parameters.Add("@Rating", dto.Rating);
                parameters.Add("@Feedback", dto.Feedback);
                parameters.Add("@CreatedBy", dto.CreatedBy);

                var rowsAffected = await CreateConnection().ExecuteAsync(
                    "usp_UpdateJobCompletionByCustomer",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new ActionResults
                {
                    Success = true,
                    Id = null,
                    Message = "Job Status updated successfully."
                };
            }
            catch (Exception ex)
            {

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
        public async Task<ActionResults> UpdateJobStatus(JobStatusUpdateDto dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", dto.JobId);
                parameters.Add("@Notes", dto.Notes);
                parameters.Add("@StatusId", dto.StatusId);
                parameters.Add("@Feedback", dto.Feedback);
                parameters.Add("@CreatedBy", dto.CreatedBy);
                parameters.Add("@VendorId", dto.VendorId);
                

                var rowsAffected = await CreateConnection().ExecuteAsync(
                    "usp_UpdateJobStatus",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new ActionResults
                {
                    Success = true,
                    Id = null,
                    Message = "Job Status updated successfully."
                };
            }
            catch (Exception ex)
            {

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
        public async Task<JobStatusTrackingResponse> GetJobStatusTrackingAsync(long jobId)
        {
            using var connection = CreateConnection();

            var statusTracking = await connection.QueryAsync<JobStatusTrackingDto>(
                "usp_GetJobStatusTrackingByJobId",
                new { JobId = jobId },
                commandType: CommandType.StoredProcedure
            );

            var partyInfo = await connection.QueryAsync<JobPartyInfoDto>(
                @"SELECT TOP 1 
              jea.Name AS CustomerName,
              jea.Longitude,
              jea.Latitude,
              jea.Mobile,
              jev.Name AS VendorName,
              jev.Longitude AS VendorLongitude,
              jev.Latitude AS VendorLatitude,
              jev.Mobile AS VendorMobile,
              jeaa.EmployeePhoneNumber,
              jeaa.EmployeeName 
          FROM JobApproval ja
          INNER JOIN Customers jea ON jea.CustomerId = ja.CustomerId
          INNER JOIN Customers jev ON jev.CustomerId = ja.VendorId
          LEFT JOIN JobEmployeeAssignment jeaa ON ja.JobId = jeaa.JobId
          WHERE ja.JobId = @JobId",
                new { JobId = jobId }
            );

            return new JobStatusTrackingResponse
            {
                StatusTracking = statusTracking,
                PartyInfo = partyInfo
            };
        }

        public async Task<IEnumerable<VendorHistoryListDto>> GetVendorHistoryListAsync(long vendorId)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<VendorHistoryListDto>(
                "usp_GetVendorHistoryList",
                new { VendorId = vendorId },
                commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<HistoryDetailDto> GetVendorHistoryDetailAsync(long vendorId, long? jobId)
        {
            try
            {

          
            using (var connection = CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(
                "usp_GetVendorHistoryDetail",
                new { VendorId = vendorId, JobId = jobId },
                commandType: CommandType.StoredProcedure))
            {
                var jobDetail = await multi.ReadFirstOrDefaultAsync<JobHistoryDto>();
                var completionDetails = (await multi.ReadAsync<JobCompletionDetailDto>()).ToList();

                return new HistoryDetailDto
                {
                    JobDetail = jobDetail,
                    CompletionDetails = completionDetails
                };
            }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CustomerHistoryListDto>> GetCustomerHistoryListAsync(long customerId)
        {
            using (var connection = CreateConnection())
            {

                return await connection.QueryAsync<CustomerHistoryListDto>(
                    "usp_GetCustomerHistoryList", new { CustomerId = customerId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<CustomerHistoryDetailDto> GetCustomerHistoryDetailAsync(long customerId, long? jobId)
        {
            try
            {

                using (var connection = CreateConnection())
                using (var multi = await connection.QueryMultipleAsync(
                    "usp_GetCustomerHistoryDetail",
                    new { CustomerId = customerId, JobId = jobId },
                    commandType: CommandType.StoredProcedure))
                {
                    var jobDetail = await multi.ReadFirstOrDefaultAsync<CustomerHistoryDetails>();
                    var completionDetails = (await multi.ReadAsync<JobCompletionDetailDto>()).ToList();

                    return new CustomerHistoryDetailDto
                    {
                        JobDetail = jobDetail,
                        CompletionDetails = completionDetails
                    };

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<ServiceDetailDto> GetServiceDetailAsync(long vendorId, long serviceId)
        {
            {
                using (var connection = CreateConnection())
                {
                    using var multi = await connection.QueryMultipleAsync("usp_GetServiceDetail",
                new { VendorId = vendorId, ServiceId = serviceId }, commandType: CommandType.StoredProcedure);

                    // 1. Vendors
                    var vendorList = (await multi.ReadAsync<VendorDetailDto>()).ToList();

                    // 2. Reviews
                    var reviews = (await multi.ReadAsync<CustomerReviewDto>()).ToList();
                    var reviewcount = (await multi.ReadAsync<CustomerReviewCount>()).ToList();

                    // Map reviews to vendors
                    foreach (var vendor in vendorList)
                    {
                        vendor.Reviews = reviews.Where(r => r.JobId != 0 && r.Rating > 0 && r.Feedback != null && r.Rating > 0 && r.Feedback != null && r.Rating > 0).ToList();
                    }

                    // Service detail object
                    var serviceDetail = new ServiceDetailDto
                    {
                        VendorId = vendorId,
                        ServiceName = vendorList.FirstOrDefault()?.ServiceName, // adjust if separate Service table
                        ServiceImage = vendorList.FirstOrDefault()?.VendorLogo,
                        Vendors = vendorList,
                        Reviews= reviews,
                        ReviewCount=reviewcount,
                    };

                    return serviceDetail;
                }
            }
        }

       
    }

}
