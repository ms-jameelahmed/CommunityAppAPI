using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;

        public ServiceService(IServiceRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<ServiceMaster>> GetAllAsync() => _repo.GetAllAsync();

        public Task<ServiceMaster?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);

        public Task<long> CreateAsync(ServiceMaster service)
        {
            service.CreatedDate = DateTime.UtcNow;
            service.Deleted = false;
            return _repo.CreateAsync(service);
        }

        public Task<bool> UpdateAsync(ServiceMaster service)
        {
            service.ModifiedDate = DateTime.UtcNow;
            return _repo.UpdateAsync(service);
        }

        public Task<bool> DeleteAsync(long id) => _repo.DeleteAsync(id);

        public async Task<IEnumerable<ExploreServiceDto>> GetExploreServicesAsync(
         string search, string sortBy, decimal? minPrice, decimal? maxPrice, long? categoryId,int? pagenumber, [FromQuery] int? records)
        {
            return await _repo.GetExploreServicesAsync(search, sortBy, minPrice, maxPrice, categoryId,pagenumber,records);
        }

    }

}
