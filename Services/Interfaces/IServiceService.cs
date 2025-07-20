using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceMaster>> GetAllAsync();
        Task<ServiceMaster?> GetByIdAsync(long id);
        Task<long> CreateAsync(ServiceMaster service);
        Task<bool> UpdateAsync(ServiceMaster service);
        Task<bool> DeleteAsync(long id);
    }

}
