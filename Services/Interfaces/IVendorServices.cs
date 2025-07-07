using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IVendorService
    {
        Task<string> RegisterVendorAsync(Vendor vendor);
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<Vendor> GetByIdAsync(long vendorId);
        Task<string> UpdateVendorAsync(Vendor vendor);
        Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy);
    }
}