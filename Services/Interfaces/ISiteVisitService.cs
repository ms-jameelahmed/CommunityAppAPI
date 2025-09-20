using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface ISiteVisitService
    {
        Task<int> CreateSiteVisitRequestAsync(SiteVisitRequestDto request);
        Task<ActionResults> AssignEmployeesAsync(AssignSiteVisitEmployeeRequest request);
        Task<SiteVisitDetailDto> GetSiteVisitDetailAsync(int siteVisitId);
        Task<ActionResults> UpdateCustomerResponseAsync(int siteVisitId, bool isAccepted);
        Task<bool> ValidateQRCodeAsync(string qrCode);
        Task<EmailResponseDto> BuildEmailContentAsync(GatePassRequest request);
        Task<GatePassRequest> GetGatePassRequestByIdAsync(long jobId, long siteVisitId);
    }
}
