using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class CommonRepository: ICommonRepository
    {
        private readonly IConfiguration _config;

        public CommonRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));


        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var rowsAffected = await CreateConnection().ExecuteAsync(
                "usp_ChangePassword",
                new { dto.UserId, dto.OldPassword, dto.NewPassword },
                commandType: CommandType.StoredProcedure
            );
            return rowsAffected > 0;
        }

        public async Task<bool> ForgotPasswordAsync(string email, string resetToken)
        {
            var rowsAffected = await CreateConnection().ExecuteAsync(
                "usp_ForgotPassword_InsertToken",
                new { Email = email, Token = resetToken },
                commandType: CommandType.StoredProcedure
            );
            return rowsAffected > 0;
        }

     
       
        public async Task<(string Id, string Email, string UserType)> GetUserByEmailAsync(string email, string userType)
        {
            return await CreateConnection().QueryFirstOrDefaultAsync<(string, string, string)>(
                "usp_GetUserByEmail",
                new { Email = email, UserType = userType },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<SmtpSettingsDto> GetSmtpSettingsAsync()
        {
            return await CreateConnection().QueryFirstOrDefaultAsync<SmtpSettingsDto>(
                "usp_GetSmtpSettings",
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task InsertNotificationHistoryAsync(NotificationHistoryDto dto)
        {
            await CreateConnection().ExecuteAsync("usp_InsertNotificationHistory", dto, commandType: CommandType.StoredProcedure);
        }
        public async Task<ForgotPasswordTokenDto> GenerateTokenAsync(string email)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ForgotPasswordTokenDto>(
                "usp_GeneratePasswordResetToken",
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );
        }

        public Task<IEnumerable<JobHistoryDto>> GetJobHistoryAsync(string userId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            using var connection = CreateConnection();
            var userId = await connection.QueryFirstOrDefaultAsync<string?>(
                "SELECT UserId FROM LoginDetails ld    INNER JOIN Customers c ON c.CustomerId = ld.CustomerId   WHERE c.Email = @Email",
                new { Email = email }
            );
            return userId;
        }

        public async Task InsertForgotPasswordOtpAsync(string userId, string otp, DateTime expiry)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_InsertForgotPasswordOTP",
                new { UserId = userId, OTP = otp, ExpiryTime = expiry },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> VerifyForgotPasswordOtpAsync(string userId, string otp)
        {
            using var connection = CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int>(
                "usp_VerifyForgotPasswordOTP",
                new { UserId = userId, OTP = otp },
                commandType: CommandType.StoredProcedure
            );
            return result == 1;
        }

        public async Task MarkOtpAsUsedAsync(string userId, string otp)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_MarkOTPAsUsed",
                new { UserId = userId, OTP = otp },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdatePasswordAsync(string userId, string hashedPassword)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "UPDATE LoginDetails SET Password = @Password WHERE UserId = @UserId",
                new { UserId = userId, Password = hashedPassword }
            );
        }

        public async Task<IEnumerable<JobStatusDto>> GetJobStatusesAsync()
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<JobStatusDto>(
                "usp_GetJobStatuses",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task SaveDeviceTokenAsync(string userId, string token)
        {
            var parameters = new { UserId = userId, Token = token };
            await CreateConnection().ExecuteAsync(
                "usp_SaveDeviceToken", // Stored Procedure Name
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<string> GetDeviceTokenAsync(long customerId)
        {
            return await CreateConnection().QueryFirstOrDefaultAsync<string>(
                "usp_GetDeviceTokenByUserId",
                new { UserId = customerId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<string>> GetUserTokensByJobIdAsync(long? jobId,string type)
        {
            return await CreateConnection().QueryAsync<string>(
               "usp_GetDeviceTokensByJobId",
               new { JobId = jobId , Type = type },
               commandType: CommandType.StoredProcedure
           );
            
        }
    }
}
