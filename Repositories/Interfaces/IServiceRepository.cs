using CommunityAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<ServiceMaster>> GetAllAsync();
        Task<ServiceMaster?> GetByIdAsync(long id);
        Task<long> CreateAsync(ServiceMaster service);
        Task<bool> UpdateAsync(ServiceMaster service);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<ExploreServiceDto>> GetExploreServicesAsync(
   string search, string sortBy, decimal? minPrice, decimal? maxPrice, long? categoryId, int? pagenumber, [FromQuery] int? records);


    }

}
