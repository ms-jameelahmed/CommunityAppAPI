using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface ICommunityService
    {
        Task<IEnumerable<CommunityMaster>> GetAllAsync();
        Task<CommunityMaster?> GetByIdAsync(int id);
        Task<int> CreateAsync(CommunityMaster community);
        Task<bool> UpdateAsync(CommunityMaster community);
        Task<bool> DeleteAsync(int id);
    }


}
