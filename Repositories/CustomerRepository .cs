using Azure;
using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace CommunityAppAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _db;

        public CustomerRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _db.QueryAsync<Customer>("usp_Customers_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<Customer> GetByIdAsync(long id)
        {
            return await _db.QueryFirstOrDefaultAsync<Customer>("usp_Customers_GetById", new { CustomerId = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<ActionResults> AddAsync(Customer customer)
        {
            try
            {
                var result = await _db.QueryAsync<string>("usp_Customers_Insert", new
                {
                    customer.TypeId,
                    customer.Type,
                    customer.Name,
                    customer.Mobile,
                    customer.Landline,
                    customer.AlternateContactNo,
                    customer.Email,
                    customer.CommunityId,
                    customer.Building,
                    customer.Block,
                    customer.Address,
                    customer.Longitude,
                    customer.Latitude,

                    customer.SettlementPercentage,

                    customer.CreatedBy,
                    customer.UserId,
                    customer.Password,

                    customer.CustomerType,
                    customer.Image,
                }, commandType: CommandType.StoredProcedure);

                var response = result.FirstOrDefault();

                if (response?.StartsWith("UserId") == true ||
                    response?.StartsWith("Email") == true ||
                    response?.StartsWith("Mobile") == true)
                {

                    return new ActionResults
                    {
                        Success = false,
                        Id = 0,
                        Message = response
                    };

                }
                return new ActionResults
                {
                    Success = true,
                    Id = 0,
                    Message = "Vendor registered successfully."
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



        public async Task UpdateAsync(UpdateCustomer customer)
        {

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

            await _db.ExecuteAsync(
                "usp_Customers_Update",
                parameters,
                commandType: CommandType.StoredProcedure);


        }

        public async Task DeleteAsync(long id, string modifiedBy)
        {
            await _db.ExecuteAsync("usp_Customers_Delete",
                new { CustomerId = id, ModifiedDate = DateTime.Now, ModifiedBy = modifiedBy },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<CustomerDashboardDto> GetCustomerDashboardAsync(int customerId)
        {

            using var multi = await _db.QueryMultipleAsync("usp_GetCustomerDashboard", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);


            var quotationSummary = await multi.ReadFirstOrDefaultAsync<CustomerSummaryDto>();
            var promotional = await multi.ReadFirstOrDefaultAsync<List<PromotionalContentDto>>();


            return new CustomerDashboardDto
            {
                jobstats = quotationSummary,
                Promotionalcontent = promotional,

            };
        }
    }
}
