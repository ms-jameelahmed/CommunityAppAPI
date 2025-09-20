using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommunityAppAPI.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly IConfiguration _config;

        public VendorRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        public async Task<IEnumerable<Vendor>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var vendors = await connection.QueryAsync<Vendor>("usp_Vendor_GetAll", commandType: CommandType.StoredProcedure);
            return vendors;
        }

        public async Task<Vendor> GetByIdAsync(long id)
        {
            using var multi = await CreateConnection().QueryMultipleAsync("usp_Vendor_GetById",
                new { VendorId = id }, commandType: CommandType.StoredProcedure);

            var vendor = await multi.ReadFirstOrDefaultAsync<Vendor>();
            var documents = (await multi.ReadAsync<VendorDocument>()).ToList();
            var bankDetail = await multi.ReadFirstOrDefaultAsync<BankDetail>();

            if (vendor != null)
            {
                vendor.Documents = documents;
                vendor.BankDetail = bankDetail;
            }

            return vendor;
        }

        public async Task AddAsync(Vendor vendor)
        {
            // Optional: if you don't want RegisterVendorAsync and AddAsync both
            await RegisterVendorAsync(vendor);
        }



        public async Task<ActionResults> RegisterVendorAsync(Vendor vendor)
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Check for duplicates or insert
                var result = await connection.QueryAsync<string>(
                    "usp_Vendor_Insert",
                    new
                    {
                        vendor.TypeId,
                        vendor.Type,
                        vendor.Name,
                        vendor.Mobile,
                        vendor.Landline,
                        vendor.AlternateContactNo,
                        vendor.Email,
                        vendor.CommunityId,
                        vendor.Building,
                        vendor.Block,
                        vendor.Address,
                        vendor.Longitude,
                        vendor.Latitude,
                        vendor.Blacklisted,
                        vendor.SettlementPercentage,
                        vendor.CreatedBy,
                        vendor.UserId,
                        vendor.Password,
                        vendor.CustomerType,
                        vendor.Image,
                    },
                    transaction,
                    commandType: CommandType.StoredProcedure
                );

                var response = result.FirstOrDefault();

                if (response?.StartsWith("UserId") == true ||
                    response?.StartsWith("Email") == true ||
                    response?.StartsWith("Mobile") == true)
                {
                    transaction.Rollback();
                    return new ActionResults
                    {
                        Success = false,
                        Id = 0,
                        Message = response
                    };
                    
                }

                long vendorId = long.Parse(response);

                if (vendor.BankDetail != null)
                {
                    await connection.ExecuteAsync(
                        "usp_VendorBank_Insert",
                        new
                        {
                            VendorId = vendorId,
                            vendor.BankDetail.BankName,
                            vendor.BankDetail.AccountNumber,
                            vendor.BankDetail.IBAN,
                            vendor.BankDetail.SWIFTBIC,
                            vendor.BankDetail.BankBranch,
                            vendor.BankDetail.Address,
                           
                            vendor.CreatedBy,
                        },
                        transaction,
                        commandType: CommandType.StoredProcedure
                    );
                }

                if (vendor.Documents != null && vendor.Documents.Any())
                {
                    foreach (var doc in vendor.Documents)
                    {
                        await connection.ExecuteAsync(
                            "usp_VendorDocument_Insert",
                            new
                            {
                                VendorId = vendorId,
                                doc.DocumentTypeId,
                                doc.DocumentExpiryDate,
                                doc.DocumentFile,
                                 doc.DocumentNumber,
                                vendor.CreatedBy,
                            },
                            transaction,
                            commandType: CommandType.StoredProcedure
                        );
                    }
                }

                transaction.Commit();
                return new ActionResults
                {
                    Success = true,
                    Id = vendorId,
                    Message = "Vendor registered successfully."
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

        public async Task<string> UpdateVendorAsync(UpdateCustomer customer)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", customer.CustomerId);
            parameters.Add("@TypeId", customer.TypeId);
            parameters.Add("@Type", customer.Type);
            parameters.Add("@Name", customer.Name);
            parameters.Add("@Mobile", customer.Mobile);
            parameters.Add("@Landline", customer.Landline);
            parameters.Add("@AlternateContactNo", customer.AlternateContactNo);
            parameters.Add("@Email", customer.Email);
            parameters.Add("@CommunityId", customer.CommunityId);
            parameters.Add("@Building", customer.Building);
            parameters.Add("@Block", customer.Block);
            parameters.Add("@Address", customer.Address);
            parameters.Add("@Longitude", customer.Longitude);
            parameters.Add("@Latitude", customer.Latitude);
            parameters.Add("@Blacklisted", customer.Blacklisted);
            parameters.Add("@SettlementPercentage", customer.SettlementPercentage);
            parameters.Add("@Active", customer.Active);
            parameters.Add("@ModifiedBy", customer.ModifiedBy);
            parameters.Add("@Image",
    customer.Image != null && customer.Image.Length > 0
        ? customer.Image
        : null,
    DbType.Binary);

            await connection.ExecuteAsync(
                "usp_Vendor_Update",
                parameters,
                commandType: CommandType.StoredProcedure);

            return $"Success: Vendor Details updated";
        }

        public async Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy)
        {
            using var connection = CreateConnection();

            var rows = await connection.ExecuteAsync(
                "usp_Vendor_Delete",
                new { VendorId = vendorId, ModifiedBy = modifiedBy },
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }

        public async Task<IEnumerable<Vendor_Service>> GetVendorServicesAsync(long vendorId)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<Vendor_Service>("usp_GetVendorServices" ,new { VendorId = vendorId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<ActionResults> InsertVendorServiceAsync(Vendor_Service vendorService)
        {
            using var connection = CreateConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@VendorId", vendorService.VendorId);
                parameters.Add("@ServiceId", vendorService.ServiceId);
                parameters.Add("@Active", vendorService.Active);
                parameters.Add("@CreatedBy", vendorService.CreatedBy);
                parameters.Add("@Image", vendorService.Image);
                parameters.Add("@Description", vendorService.Description);
                var response = await connection.ExecuteAsync("usp_InsertVendorService", parameters, commandType: CommandType.StoredProcedure);


                return new ActionResults
                {
                    Success = true,
                    Id = 0,
                    Message = "Vendor Service registered successfully."
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

        public async Task<ActionResults> UpdateVendorServiceAsync(Vendor_Service vendorService)
        {
            using var connection = CreateConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IIdentity", vendorService.IIdentity);
                parameters.Add("@VendorId", vendorService.VendorId);
                parameters.Add("@ServiceId", vendorService.ServiceId);
                parameters.Add("@Active", vendorService.Active);
                parameters.Add("@Deleted", vendorService.Deleted);
                parameters.Add("@ModifiedBy", vendorService.ModifiedBy);
                parameters.Add("@Description", vendorService.Description);
                if (vendorService.Image != null)
                    parameters.Add("@Image", vendorService.Image, DbType.Binary);
                else
                    parameters.Add("@Image", null, DbType.Binary);


                var response = await connection.ExecuteAsync("usp_UpdateVendorService", parameters, commandType: CommandType.StoredProcedure);


                return new ActionResults
                {
                    Success = true,
                    Id = 0,
                    Message = "Vendor Service Updated successfully."
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

        public async Task<IEnumerable<Vendor_Service>> GetAllVendorsforService(long _ServiceId)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<Vendor_Service>("usp_GetAllVendorsforService", new { ServiceId = _ServiceId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<VendorDashboardDto> GetVendorDashboardAsync(int vendorId)
        {
            using var connection = CreateConnection();
            using var multi = await connection.QueryMultipleAsync("usp_GetVendorDashboard", new { VendorId = vendorId }, commandType: CommandType.StoredProcedure);

            var documents = (await multi.ReadAsync<VendorDocument>()).ToList();
            var quotationSummary = await multi.ReadFirstOrDefaultAsync<VendorSummaryDto>();
            var jobStatusCounts = (await multi.ReadAsync<JobStatusCategoryDto>()).ToList();

            return new VendorDashboardDto
            {
                Documents = documents,
                Stats = quotationSummary,
                JobStatusCounts = jobStatusCounts
            };
        }


    }


}
