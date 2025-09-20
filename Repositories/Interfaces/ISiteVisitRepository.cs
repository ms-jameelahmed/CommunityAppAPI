using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface ISiteVisitRepository
    {
        Task<int> CreateSiteVisitRequestAsync(SiteVisitRequestDto request);
        Task<ActionResults> AssignEmployeesAsync(AssignSiteVisitEmployeeRequest request);
        Task<SiteVisitDetailDto> GetSiteVisitDetailAsync(int siteVisitId);
        Task<ActionResults> UpdateCustomerResponseAsync(int siteVisitId, bool isAccepted);
        Task<bool> ValidateQRCodeAsync(string qrCode);
        Task<GatePassRequest> GetGatePassRequestByIdAsync(long jobId, long siteVisitId);
    }
}
