namespace CommunityAppAPI.Models
{
    public class Common
    {
    }

    public class ChangePasswordDto
    {
        public string UserId { get; set; }


        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
 

    public class ForgotPasswordEmailDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class ForgotPasswordTokenDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
   

     
    public class EmailAttachmentDto
    {
        public string FileName { get; set; }
        public byte[] FileBase64 { get; set; }
    }

    public class EmailRequestDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class SmtpSettingsDto
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
    }
    public class NotificationHistoryDto
    {
        public string UserId { get; set; }
        public string UserType { get; set; } // "Customer" or "Vendor"
        public string Email { get; set; }
        public string NotificationType { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool SentStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string UserType { get; set; }
    }
    public class JobHistoryDto
    {
        public long JobId { get; set; }
        public string JobTitle { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AssignedEmployee { get; set; }

        public decimal TotalAmount { get; set; }
        public string Feedback { get; set; }
        public string Remarks { get; set; }
        public int Rating { get; set; }

    }

    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
    }

    public class VerifyOtpDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
    }

    public class JobStatusDto
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int ProgressPercent { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
    public class SendEmailRequestDto
    {
        public string FromEmail { get; set; } // Optional - will use DB value if null
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
    public class JobStatusTrackingDto
    {
        public long JobStatusUpdateId { get; set; }
        public long JobId { get; set; }
        public long? VendorId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int ProgressPercent { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string CustomerRemarks { get; set; }
        public int WorkDonePercentage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DeviceTokenDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }

    public class NotificationRequest
    {
        public long customerId { get; set; }
        public string? Token { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public string? UserId { get; set; }
    }
}
