using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface ICommonRepository
    {
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<bool> ForgotPasswordAsync(string email, string resetToken);
        Task<IEnumerable<JobHistoryDto>> GetJobHistoryAsync(string userId, string role);

        Task<string?> GetUserIdByEmailAsync(string email);
        Task InsertForgotPasswordOtpAsync(string userId, string otp, DateTime expiry);
        Task<bool> VerifyForgotPasswordOtpAsync(string userId, string otp);
        Task MarkOtpAsUsedAsync(string userId, string otp);
        Task UpdatePasswordAsync(string userId, string hashedPassword);
        Task<IEnumerable<JobStatusDto>> GetJobStatusesAsync();

        Task SaveDeviceTokenAsync(string userId, string token);
        Task<string> GetDeviceTokenAsync(long customerId);
        Task<IEnumerable<string>> GetUserTokensByJobIdAsync(long? jobId, string type);
    }
}
