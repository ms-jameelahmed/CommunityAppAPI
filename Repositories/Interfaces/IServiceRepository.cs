using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<ServiceMaster>> GetAllAsync();
        Task<ServiceMaster?> GetByIdAsync(long id);
        Task<long> CreateAsync(ServiceMaster service);
        Task<bool> UpdateAsync(ServiceMaster service);
        Task<bool> DeleteAsync(long id);
    }

}
