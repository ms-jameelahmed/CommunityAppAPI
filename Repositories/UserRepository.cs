using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;

namespace CommunityAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User ValidateUser(string email, string password)
        {
            //Third Push Test
            // Second Push
            //sample code from AH. Dated: 08  jul 2025
            // Mock user validation
            if (email == "admin@example.com" && password == "password")
                return new User { Id = 1, Email = email };
            return null;
        }
    }
}
