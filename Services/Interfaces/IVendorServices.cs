using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IVendorService
    {
        Task<bool> RegisterVendorAsync(Vendor vendor);
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<Vendor> GetByIdAsync(long vendorId);
        Task<bool> UpdateVendorAsync(Vendor vendor);
        Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy);
    }
}