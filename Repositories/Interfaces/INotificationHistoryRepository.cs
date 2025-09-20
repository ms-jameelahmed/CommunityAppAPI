using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface INotificationHistoryRepository
    {
        Task LogNotificationAsync(NotificationHistoryDto history);
    }
}
