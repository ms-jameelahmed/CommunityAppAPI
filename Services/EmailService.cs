using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace CommunityAppAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ICommonRepository _userRepository;
        private readonly ISmtpSettingsRepository _settingsRepository;

        public EmailService(IConfiguration config, ICommonRepository userRepository, ISmtpSettingsRepository settingsRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _settingsRepository = settingsRepository;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSettings = await CreateConnection().QueryFirstOrDefaultAsync<dynamic>(
                "SELECT TOP 1 Host, Port, FromEmail, Password FROM SmtpSettings WHERE Active = 1"
            );

            using var smtpClient = new SmtpClient((string)smtpSettings.Host)
            {
                Port = (int)smtpSettings.Port,
                Credentials = new NetworkCredential((string)smtpSettings.FromEmail, (string)smtpSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage((string)smtpSettings.FromEmail, toEmail, subject, body);
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);


        }

        public async Task<bool> SendEmailAsync(SendEmailRequestDto request)
        {
            var smtpSettings = await _settingsRepository.GetSmtpSettingsAsync();
            if (smtpSettings == null)
                throw new Exception("SMTP settings not configured.");

            string fromEmail = string.IsNullOrEmpty(request.FromEmail)
                ? smtpSettings.FromEmail
                : request.FromEmail;

            using var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
            {
                Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                EnableSsl = smtpSettings.EnableSsl
            };

            var mailMessage = new MailMessage(fromEmail, request.ToEmail, request.Subject, request.Body)
            {
                IsBodyHtml = request.IsHtml
            };

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
    }
}
