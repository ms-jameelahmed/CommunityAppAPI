using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class SmtpSettingsRepository: ISmtpSettingsRepository
    {
        private readonly IDbConnection _connection;

        public SmtpSettingsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<SmtpSettingsDto> GetSmtpSettingsAsync()
        {
            return await _connection.QueryFirstOrDefaultAsync<SmtpSettingsDto>(
                "usp_GetSmtpSettings",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
