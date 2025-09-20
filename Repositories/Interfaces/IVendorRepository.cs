using CommunityAppAPI.Models;
using CommunityAppAPI.Services;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IVendorRepository
    {
        Task<ActionResults> RegisterVendorAsync(Vendor vendor);
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<Vendor> GetByIdAsync(long vendorId);
        Task<string> UpdateVendorAsync(UpdateCustomer vendor);
        Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy);

        Task<IEnumerable<Vendor_Service>> GetVendorServicesAsync(long vendorId);
        Task<ActionResults> InsertVendorServiceAsync(Vendor_Service vendorService);
        Task<ActionResults> UpdateVendorServiceAsync(Vendor_Service vendorService);

        Task<IEnumerable<Vendor_Service>> GetAllVendorsforService(long _ServiceId);

        Task<VendorDashboardDto> GetVendorDashboardAsync(int vendorId);
    }

}
