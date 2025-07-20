using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
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
                                doc.DocumentType,
                                doc.DocumentExpiryDate,
                                doc.DocumentFile,
                               
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

        public async Task<string> UpdateVendorAsync(Vendor vendor)
        {
            using var connection = CreateConnection();

            var rows = await connection.ExecuteAsync(
                "usp_Vendor_Update",
                new
                {
                    vendor.CustomerId,
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
                    vendor.Active,
                    vendor.ModifiedDate,
                    vendor.ModifiedBy
                },
                commandType: CommandType.StoredProcedure
            );

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
    }


}
