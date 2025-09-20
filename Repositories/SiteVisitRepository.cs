using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class SiteVisitRepository : ISiteVisitRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;

        public SiteVisitRepository(IConfiguration config, IDbConnection dbConnection)
        {
            _config = config;
            _dbConnection = dbConnection;
        }


        public async Task<int> CreateSiteVisitRequestAsync(SiteVisitRequestDto request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", request.JobId);
            parameters.Add("@VendorId", request.VendorId);
            parameters.Add("@CustomerId", request.CustomerId);
            parameters.Add("@RequestedDate", request.RequestedDate);
            parameters.Add("@SiteVisitId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("usp_CreateSiteVisitRequest", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@SiteVisitId");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        //public async Task<ActionResults> AssignEmployeesAsync(AssignSiteVisitEmployeeRequest request)
        //{
        //    try
        //    {
        //        var connection = (SqlConnection)CreateConnection();
        //        await connection.OpenAsync();
        //        var employeeTable = new DataTable();
        //        employeeTable.Columns.Add("EmployeeName", typeof(string));
        //        employeeTable.Columns.Add("EmployeePhoneNumber", typeof(string));
        //        employeeTable.Columns.Add("EmiratesId", typeof(string));
        //        employeeTable.Columns.Add("EmiratesIdPhoto", typeof(byte[])); // must match VARBINARY(MAX)
        //        foreach (var item in request.AssignEmployeeList)
        //        {
        //            byte[]? photoBytes = null;

        //            if (!string.IsNullOrWhiteSpace(item.EmiratesIdPhoto))
        //            {
        //                try
        //                {
        //                    photoBytes = Convert.FromBase64String(item.EmiratesIdPhoto);
        //                }
        //                catch (FormatException)
        //                {
        //                    // Handle base64 decode failure gracefully (optional)
        //                    photoBytes = null;
        //                }
        //            }

        //            employeeTable.Rows.Add(
        //                item.EmployeeName,
        //                item.EmployeePhoneNumber,
        //                item.EmiratesIdNumber,
        //                photoBytes
        //            );
        //        }

        //        var parameters = new DynamicParameters();

        //        using (var command = new SqlCommand("usp_AssignEmployeeForSiteVisit", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            var tvpParam = new SqlParameter("@AssignEmployeeList", SqlDbType.Structured)
        //            {
        //                TypeName = "dbo.JobEmployeeAssignment", // exact type name in SQL Server
        //                Value = employeeTable
        //            };

        //            command.Parameters.Add(tvpParam);
        //            command.Parameters.AddWithValue("@SiteVisitId", request.SiteVisitId);


        //            await command.ExecuteNonQueryAsync();
        //            connection.Close();
        //        }


        //        return new ActionResults
        //        {
        //            Success = true,
        //            Id = null,
        //            Message = "Employees assigned successfully."
        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //        var errorId = Guid.NewGuid();
        //        var timestamp = DateTime.UtcNow.ToString("u");
        //        return new ActionResults
        //        {
        //            Success = false,
        //            Id = 0,
        //            Message = $"Error: {ex.Message} | Reference ID: {errorId} | Timestamp: {timestamp}"
        //        };
        //    }
        //}

        public async Task<ActionResults> AssignEmployeesAsync(AssignSiteVisitEmployeeRequest request)
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
                        photoBytes,
                        item.EmployeeEmail
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

        public async Task<SiteVisitDetailDto> GetSiteVisitDetailAsync(int jobId)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using (var multi = await connection.QueryMultipleAsync("usp_GetSiteVisitDetail", new { JobId = jobId }, commandType: CommandType.StoredProcedure))
            {
                var siteVisit = await multi.ReadFirstOrDefaultAsync<SiteVisitDetailDto>();
                if (siteVisit != null)
                {
                    var employees = (await multi.ReadAsync<AssignedEmployeeDto>()).ToList();
                    siteVisit.Employees = employees;
                }
                return siteVisit;
            }

        }

        public async Task<ActionResults> UpdateCustomerResponseAsync(int siteVisitId, bool isAccepted)
        {
            var rows = await _dbConnection.ExecuteAsync("usp_UpdateCustomerSiteVisitResponse",
                new { SiteVisitId = siteVisitId, IsAccepted = isAccepted }, commandType: CommandType.StoredProcedure);

            return new ActionResults
            {
                Success = true,
                Id = null,
                Message = "Site Visit Status updated successfully."
            };
        }

        public async Task<bool> ValidateQRCodeAsync(string qrCode)
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int>(
                "usp_ValidateQRCode", new { QRCode = qrCode }, commandType: CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<GatePassRequest> GetGatePassRequestByIdAsync(long jobId, long siteVisitId)
        {
            using var connection = CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<GatePassRequest>(
                "usp_GetGatePassRequests",
                new { JobId = jobId, SiteVisitId= siteVisitId },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
}
