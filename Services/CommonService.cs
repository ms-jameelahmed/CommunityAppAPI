using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace CommunityAppAPI.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _userRepository;
        private readonly IEmailService _emailService;

        public CommonService(ICommonRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-service-account.json")
                });
            }
        }


        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            return await _userRepository.ChangePasswordAsync(dto);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var token = Guid.NewGuid().ToString();
            var success = await _userRepository.ForgotPasswordAsync(dto.Email, token);

            if (success)
            {
                var resetLink = $"https://yourapp.com/reset-password?token={token}";
                await _emailService.SendEmailAsync(dto.Email, "Password Reset", $"Click here to reset your password: {resetLink}");
            }
            return success;
        }

        public async Task<IEnumerable<JobHistoryDto>> GetJobHistoryAsync(string userId, string role)
        {
            return await _userRepository.GetJobHistoryAsync(userId, role);
        }

        public async Task SendForgotPasswordOtpAsync(ForgotPasswordRequestDto dto)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(dto.Email);
            if (userId == null)
                throw new Exception("Email not found.");

            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(10);

            await _userRepository.InsertForgotPasswordOtpAsync(userId, otp, expiry);

            await _emailService.SendEmailAsync(dto.Email, "Password Reset OTP", $"Your OTP is {otp}. Valid for 10 minutes.");
        }

        public async Task<bool> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(dto.Email);
            if (userId == null)
                return false;

            var isValid = await _userRepository.VerifyForgotPasswordOtpAsync(userId, dto.OTP);
            if (isValid)
            {
                await _userRepository.MarkOtpAsUsedAsync(userId, dto.OTP);
            }
            return isValid;
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(dto.Email);
            if (userId == null)
                throw new Exception("Email not found.");

            var isValid = await _userRepository.VerifyForgotPasswordOtpAsync(userId, dto.OTP);
            if (!isValid)
                throw new Exception("Invalid or expired OTP.");

            await _userRepository.MarkOtpAsUsedAsync(userId, dto.OTP);

            var hashedPassword = (dto.NewPassword);
            await _userRepository.UpdatePasswordAsync(userId, hashedPassword);
        }

        public async Task<IEnumerable<JobStatusDto>> GetJobStatusesAsync()
        {
            return await _userRepository.GetJobStatusesAsync();
        }

        public async Task RegisterTokenAsync(string userId, string token)
        {
            await _userRepository.SaveDeviceTokenAsync(userId, token);
        }

        public async Task<string> SendNotificationAsync(long customerId, string title, string body)
        {
            var token = await _userRepository.GetDeviceTokenAsync(customerId);
            if (string.IsNullOrEmpty(token))
                throw new Exception("No device token found for user.");

            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<bool> SendMultpleNotificationByJobAsync(long? jobId, string title, string body, string type)
        {
            // 1. Get tokens from repository based on JobId
            var tokens = await _userRepository.GetUserTokensByJobIdAsync(jobId,type);

            if (tokens == null || !tokens.Any())
                return false;

            // 2. Send notifications
            foreach (var token in tokens)
            {
                var message = new Message()
                {
                    Token = token,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    }
                };

                  await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }

            return true;
        }
    }
}
