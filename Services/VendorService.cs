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

        public Task<bool> RegisterVendorAsync(Vendor vendor)
            => _repo.RegisterVendorAsync(vendor);

        public Task<IEnumerable<Vendor>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Vendor> GetByIdAsync(long vendorId)
            => _repo.GetByIdAsync(vendorId);

        public Task<bool> UpdateVendorAsync(Vendor vendor)
            => _repo.UpdateVendorAsync(vendor);

        public Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy)
            => _repo.DeleteVendorAsync(vendorId, modifiedBy);
    }
}
