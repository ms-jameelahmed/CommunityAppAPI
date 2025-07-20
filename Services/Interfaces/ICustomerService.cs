using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(long id);
        Task<ActionResults> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(long id, string modifiedBy);
    }
}
