using CommunityAppAPI.Models;
using CommunityAppAPI.Services;
using CommunityAppAPI.Services.Common;
using CommunityAppAPI.Services.CommunityAppAPI.Services.Common;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Controllers
{
    [Route("commonapi/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        
        private readonly ICommonService _userService;
        private readonly IEmailService _emailService;

        private readonly IResponseService _responseService; // Your reusable response wrapper

        public CommonController(ICommonService userService, IResponseService responseService, IEmailService emailService)
        {
            _userService = userService;
            _responseService = responseService;
            _emailService = emailService;
        }
        /// <summary>
        /// Change Password
        /// </summary>
        [HttpPost("change-password")]
        
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var success = await _userService.ChangePasswordAsync(dto);
                if (success)
                    return _responseService.SuccessResponse(null, "Password changed successfully.");
                else
                    return _responseService.ErrorResponse("Invalid current password or user not found.");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error changing password: {ex.Message}");
            }
        }
 
        /// <summary>
        /// Job History (Customer & Vendor)
        /// </summary>
        [HttpGet("job-history/{userId:long}")]
        public async Task<IActionResult> GetJobHistory(string userId, [FromQuery] string role)
        {
            try
            {
                var jobHistory = await _userService.GetJobHistoryAsync(userId, role);
                return _responseService.SuccessResponse(jobHistory, "Job history fetched successfully.");
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Error fetching job history: {ex.Message}");
            }
        }
        [HttpPost("forgot-password")]
        
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto dto)
        {
            try
            {
                await _userService.SendForgotPasswordOtpAsync(dto);
            return Ok(new { Message = "OTP sent to your email." });
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse($"Not able to send OTP on your email. it does not exists in system.");
            }
        }

        [HttpPost("verify-otp")]
        
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var result = await _userService.VerifyOtpAsync(dto);
            return Ok(new { IsValid = result });
        }

        [HttpPost("reset-password")]
        
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _userService.ResetPasswordAsync(dto);
            return Ok(new { Message = "Password reset successfully." });
        }
        
        [HttpPost("JobStatuses")]
        public async Task<IActionResult> GetJobStatuses()
        {
            var statuses = await _userService.GetJobStatusesAsync();
            return Ok(statuses);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDto request)
        {
            try
            {
                bool result = await _emailService.SendEmailAsync(request);
                if (result)
                    return Ok(new { success = true, message = "Email sent successfully" });

                return BadRequest(new { success = false, message = "Failed to send email" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterDeviceToken([FromBody] DeviceTokenDto dto)
        {
            // Save token in DB against the user
            await _userService.RegisterTokenAsync(dto.UserId, dto.Token);
            return Ok(new { message = "Token registered successfully" });
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var response = await _userService.SendNotificationAsync(request.customerId, request.Title, request.Body);
            return Ok(new { messageId = response });
        }
    }
}
