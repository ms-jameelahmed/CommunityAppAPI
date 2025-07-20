using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;

namespace CommunityAppAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Customer>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Customer> GetByIdAsync(long id) => _repo.GetByIdAsync(id);
        public async Task<ActionResults> AddAsync(Customer customer)
        {
            var result = await _repo.AddAsync(customer);
            return result; // could be "Email already exists" or null
        }
        public Task UpdateAsync(Customer customer) => _repo.UpdateAsync(customer);
        public Task DeleteAsync(long id, string modifiedBy) => _repo.DeleteAsync(id, modifiedBy);
    }
}
