using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IVendorRepository
    {
        Task<ActionResults> RegisterVendorAsync(Vendor vendor);
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<Vendor> GetByIdAsync(long vendorId);
        Task<string> UpdateVendorAsync(Vendor vendor);
        Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy);
    }

}
