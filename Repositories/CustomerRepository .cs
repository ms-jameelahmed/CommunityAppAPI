using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using System.Data;
using Dapper;

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

        public async Task<string> AddAsync(Customer customer)
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
                
                customer.CustomerType
            }, commandType: CommandType.StoredProcedure);

            var firstResult = result.FirstOrDefault();

            // If the first result contains a message, return it
            return firstResult; // could be null if everything is successful
        }



        public async Task UpdateAsync(Customer customer)
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
            parameters.Add("@ModifiedDate", customer.ModifiedDate);
            parameters.Add("@ModifiedBy", customer.ModifiedBy);

            await _db.ExecuteAsync("usp_Customers_Update", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(long id, string modifiedBy)
        {
            await _db.ExecuteAsync("usp_Customers_Delete",
                new { CustomerId = id, ModifiedDate = DateTime.Now, ModifiedBy = modifiedBy },
                commandType: CommandType.StoredProcedure);
        }
    }
}
