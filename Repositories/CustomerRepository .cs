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

        public async Task AddAsync(Customer customer)
        {
            await _db.ExecuteAsync("usp_Customers_Insert", customer, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _db.ExecuteAsync("usp_Customers_Update", customer, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(long id, string modifiedBy)
        {
            await _db.ExecuteAsync("usp_Customers_Delete",
                new { CustomerId = id, ModifiedDate = DateTime.Now, ModifiedBy = modifiedBy },
                commandType: CommandType.StoredProcedure);
        }
    }
}
