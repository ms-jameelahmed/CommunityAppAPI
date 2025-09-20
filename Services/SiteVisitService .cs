using Amazon.SecurityToken.Model;
using Azure.Core;
using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommunityAppAPI.Services
{
    public class SiteVisitService : ISiteVisitService
    {
        private readonly ISiteVisitRepository _repository;
        private readonly IConfiguration _config;
        public SiteVisitService(IConfiguration config, ISiteVisitRepository repository)
        {
            _config = config;
            _repository = repository;
        }
        private IDbConnection CreateConnection()
         => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        public Task<int> CreateSiteVisitRequestAsync(SiteVisitRequestDto request) => _repository.CreateSiteVisitRequestAsync(request);
        public async Task<ActionResults> AssignEmployeesAsync(AssignSiteVisitEmployeeRequest request)
        {
            var result = await _repository.AssignEmployeesAsync(request);
             
            return result;
        }
        public async Task<GatePassRequest> GetGatePassRequestByIdAsync(long jobId, long siteVisitId)
        {

            return await _repository.GetGatePassRequestByIdAsync(jobId, siteVisitId);
                 
        }
        private async Task sendemailasync(
      GatePassRequest request)
        {
            using (var message = new MailMessage())
            {
                var smtpSettings = await CreateConnection().QueryFirstOrDefaultAsync<dynamic>(
               "SELECT TOP 1 Host, Port, FromEmail, Password FROM SmtpSettings WHERE Active = 1"
           );
                var query = "SELECT Email FROM Common_Settings WHERE Active = 1";


                var emails = (await CreateConnection().QueryAsync<string>(query)).ToList();
                message.From = new MailAddress(smtpSettings.FromEmail, "dh2 gate pass system");
                foreach (var email in emails)
                {
                    message.To.Add(email);

                }
                message.CC.Add(request.VendorEmail);
                message.Subject = $"Gate Pass Request – Villa {request.VillaNumber} – {request.VisitorName}";
                var sb = new StringBuilder();

                sb.AppendLine("<p>Dear DH2 Community Security Team,</p>");
                sb.AppendLine("<p>A new gate pass request has been submitted.</p>");
                sb.AppendLine("<ul>");

                if (!string.IsNullOrEmpty(request.RequestId.ToString()))
                    sb.AppendLine($"<li><strong>Request ID:</strong> {request.RequestId}</li>");

                if (!string.IsNullOrEmpty(request.VillaNumber))
                    sb.AppendLine($"<li><strong>Villa Number:</strong> {request.VillaNumber}</li>");

                if (!string.IsNullOrEmpty(request.ReasonForVisit))
                    sb.AppendLine($"<li><strong>Reason for Visit:</strong> {request.ReasonForVisit}</li>");

                if (!string.IsNullOrEmpty(request.TradeLicense))
                    sb.AppendLine($"<li><strong>Trade License:</strong> {request.TradeLicense}</li>");

                if (!string.IsNullOrEmpty(request.VisitorName))
                    sb.AppendLine($"<li><strong>Visitor Name:</strong> {request.VisitorName}</li>");

                if (!string.IsNullOrEmpty(request.EmiratesId))
                    sb.AppendLine($"<li><strong>Emirates ID:</strong> {request.EmiratesId}</li>");

                if (!string.IsNullOrEmpty(request.VisitorMobile))
                    sb.AppendLine($"<li><strong>Visitor Mobile:</strong> {request.VisitorMobile}</li>");

                if (!string.IsNullOrEmpty(request.VisitorEmail))
                    sb.AppendLine($"<li><strong>Visitor Email:</strong> {request.VisitorEmail}</li>");

                if (!string.IsNullOrEmpty(request.CreatedBy))
                    sb.AppendLine($"<li><strong>Submitted By:</strong> {request.CreatedBy}</li>");

                sb.AppendLine($"<li><strong>Submitted On:</strong> {DateTime.Now}</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<p>Thank you,<br/>Gate Pass System</p>");

                message.Body = sb.ToString();
                message.IsBodyHtml = true;
                using var smtpClient = new SmtpClient((string)smtpSettings.Host)
                {
                    Port = (int)smtpSettings.Port,
                    Credentials = new NetworkCredential((string)smtpSettings.FromEmail, (string)smtpSettings.Password),
                    EnableSsl = true
                };

                message.IsBodyHtml = true;

                await smtpClient.SendMailAsync(message);
            }
        }
        private async Task<EmailResponseDto> BuildEmailContentAsync(GatePassRequest request)
        {
            var smtpSettings = await CreateConnection().QueryFirstOrDefaultAsync<dynamic>(
                "SELECT TOP 1 Host, Port, FromEmail, Password FROM SmtpSettings WHERE Active = 1"
            );

            var query = "SELECT Email FROM Common_Settings WHERE Active = 1";
            var emails = (await CreateConnection().QueryAsync<string>(query)).ToList();

            var subject = $"Gate Pass Request – Villa {request.VillaNumber} – {request.VisitorName}";

            var sb = new StringBuilder();
            sb.AppendLine("<p>Dear DH2 Community Security Team,</p>");
            sb.AppendLine("<p>A new gate pass request has been submitted.</p>");
            sb.AppendLine("<ul>");

            if (!string.IsNullOrEmpty(request.RequestId.ToString()))
                sb.AppendLine($"<li><strong>Request ID:</strong> {request.RequestId}</li>");

            if (!string.IsNullOrEmpty(request.VillaNumber))
                sb.AppendLine($"<li><strong>Villa Number:</strong> {request.VillaNumber}</li>");

            if (!string.IsNullOrEmpty(request.ReasonForVisit))
                sb.AppendLine($"<li><strong>Reason for Visit:</strong> {request.ReasonForVisit}</li>");

            if (!string.IsNullOrEmpty(request.TradeLicense))
                sb.AppendLine($"<li><strong>Trade License:</strong> {request.TradeLicense}</li>");

            if (!string.IsNullOrEmpty(request.VisitorName))
                sb.AppendLine($"<li><strong>Visitor Name:</strong> {request.VisitorName}</li>");

            if (!string.IsNullOrEmpty(request.EmiratesId))
                sb.AppendLine($"<li><strong>Emirates ID:</strong> {request.EmiratesId}</li>");

            if (!string.IsNullOrEmpty(request.VisitorMobile))
                sb.AppendLine($"<li><strong>Visitor Mobile:</strong> {request.VisitorMobile}</li>");

            if (!string.IsNullOrEmpty(request.VisitorEmail))
                sb.AppendLine($"<li><strong>Visitor Email:</strong> {request.VisitorEmail}</li>");

            if (!string.IsNullOrEmpty(request.CreatedBy))
                sb.AppendLine($"<li><strong>Submitted By:</strong> {request.CreatedBy}</li>");

            sb.AppendLine($"<li><strong>Submitted On:</strong> {DateTime.Now}</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("<p>Thank you,<br/>Gate Pass System</p>");

            return new EmailResponseDto
            {
                From = smtpSettings.FromEmail,
                To = emails,
                Cc = new List<string> { request.VendorEmail },
                Subject = subject,
                Body = sb.ToString()
            };
        }

        public Task<SiteVisitDetailDto> GetSiteVisitDetailAsync(int siteVisitId) => _repository.GetSiteVisitDetailAsync(siteVisitId);
        public Task<ActionResults> UpdateCustomerResponseAsync(int siteVisitId, bool isAccepted) => _repository.UpdateCustomerResponseAsync(siteVisitId, isAccepted);
        public Task<bool> ValidateQRCodeAsync(string qrCode) => _repository.ValidateQRCodeAsync(qrCode);

        Task<EmailResponseDto> ISiteVisitService.BuildEmailContentAsync(GatePassRequest request)
        {
            return BuildEmailContentAsync(request);
        }

       
    }
}
