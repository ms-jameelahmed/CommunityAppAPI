using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<CommunityMaster>> GetAllAsync();
        Task<CommunityMaster?> GetByIdAsync(int id);
        Task<int> CreateAsync(CommunityMaster community);
        Task<bool> UpdateAsync(CommunityMaster community);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<DocumentMaster>> GetDocumentTypeByIdAsync(string _type);


    }
}
