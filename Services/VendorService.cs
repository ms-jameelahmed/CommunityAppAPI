using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;

namespace CommunityAppAPI.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repo;

        public VendorService(IVendorRepository repo)
        {
            _repo = repo;
        }

        public Task<ActionResults> RegisterVendorAsync(Vendor vendor)
            => _repo.RegisterVendorAsync(vendor);

        public Task<IEnumerable<Vendor>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Vendor> GetByIdAsync(long vendorId)
            => _repo.GetByIdAsync(vendorId);

        public Task<string> UpdateVendorAsync(UpdateCustomer vendor)
            => _repo.UpdateVendorAsync(vendor);

        public Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy)
            => _repo.DeleteVendorAsync(vendorId, modifiedBy);

        public async Task<IEnumerable<Vendor_Service>> GetVendorServicesAsync(long vendorId)
        {
            return await _repo.GetVendorServicesAsync(vendorId);
        }

        public async Task<ActionResults> InsertVendorServiceAsync(Vendor_Service vendorService)
        {
            return await _repo.InsertVendorServiceAsync(vendorService);
        }

        public async Task<ActionResults> UpdateVendorServiceAsync(Vendor_Service vendorService)
        {
            return await _repo.UpdateVendorServiceAsync(vendorService);
        }

        public async Task<IEnumerable<Vendor_Service>> GetAllVendorsforService(long _ServiceId)
        {
            return await _repo.GetAllVendorsforService(_ServiceId);
        }
        public async Task<VendorDashboardDto> GetVendorDashboardAsync(int vendorId)
        {
            return await _repo.GetVendorDashboardAsync(vendorId);
        }
    }
}
