using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;

namespace CommunityAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User ValidateUser(string email, string password)
        {
            //sample code from AH
            // Mock user validation
            if (email == "admin@example.com" && password == "password")
                return new User { Id = 1, Email = email };
            return null;
        }
    }
}
