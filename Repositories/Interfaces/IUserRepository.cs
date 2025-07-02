using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User ValidateUser(string email, string password);
    }
}
