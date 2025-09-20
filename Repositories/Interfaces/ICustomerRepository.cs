using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(long id);
        Task<ActionResults> AddAsync(Customer customer);
        Task UpdateAsync(UpdateCustomer customer);
        Task DeleteAsync(long id, string modifiedBy);

        Task<CustomerDashboardDto> GetCustomerDashboardAsync(int CustomerId);
    }
}
