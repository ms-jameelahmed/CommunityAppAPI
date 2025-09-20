using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class NotificationHistoryRepository : INotificationHistoryRepository
    {
        private readonly IDbConnection _connection;

        public NotificationHistoryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task LogNotificationAsync(NotificationHistoryDto history)
        {
            await _connection.ExecuteAsync(
                "usp_InsertNotificationHistory",
                new
                {
                    history.UserId,
                    history.UserType,
                    history.Email,
                    history.NotificationType,
                    history.Subject,
                    history.Message,
                    history.SentStatus,
                    history.ErrorMessage
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
