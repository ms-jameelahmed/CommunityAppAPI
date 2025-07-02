namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IVendorRepository
    {
        Task<bool> RegisterVendorAsync(Vendor vendor);
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<Vendor> GetByIdAsync(long vendorId);
        Task<bool> UpdateVendorAsync(Vendor vendor);
        Task<bool> DeleteVendorAsync(long vendorId, string modifiedBy);
    }

}
