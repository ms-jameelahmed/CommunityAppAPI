using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(long id);
        Task<ActionResults> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(long id, string modifiedBy);
    }
}
