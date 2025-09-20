using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface ICommonService
    {

        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<IEnumerable<JobHistoryDto>> GetJobHistoryAsync(string userId, string role);

        Task SendForgotPasswordOtpAsync(ForgotPasswordRequestDto dto);
        Task<bool> VerifyOtpAsync(VerifyOtpDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task<IEnumerable<JobStatusDto>> GetJobStatusesAsync();
        Task RegisterTokenAsync(string userId, string token);
        Task<string> SendNotificationAsync(long customerId, string title, string body);
        Task<bool> SendMultpleNotificationByJobAsync(long? jobId, string title, string body, string type);
        
    }
}
