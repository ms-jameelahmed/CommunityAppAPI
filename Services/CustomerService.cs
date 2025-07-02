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
        public Task AddAsync(Customer customer) => _repo.AddAsync(customer);
        public Task UpdateAsync(Customer customer) => _repo.UpdateAsync(customer);
        public Task DeleteAsync(long id, string modifiedBy) => _repo.DeleteAsync(id, modifiedBy);
    }
}
