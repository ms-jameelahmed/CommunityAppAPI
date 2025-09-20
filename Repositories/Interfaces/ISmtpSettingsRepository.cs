using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface ISmtpSettingsRepository
    {
        Task<SmtpSettingsDto> GetSmtpSettingsAsync();
    }
}
