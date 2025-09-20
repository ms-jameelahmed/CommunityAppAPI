using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;

namespace CommunityAppAPI.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _repository;

        public CommunityService(ICommunityRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<CommunityMaster>> GetAllAsync() => _repository.GetAllAsync();

        public Task<CommunityMaster?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<int> CreateAsync(CommunityMaster community)
        {
            community.CreatedDate = DateTime.UtcNow;
            community.Deleted = false;
            return _repository.CreateAsync(community);
        }

        public Task<bool> UpdateAsync(CommunityMaster community)
        {
            community.ModifiedDate = DateTime.UtcNow;
            return _repository.UpdateAsync(community);
        }

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

        public Task<IEnumerable<DocumentMaster>> GetDocumentTypeByIdAsync(string _type) => _repository.GetDocumentTypeByIdAsync(_type);

    }

}
