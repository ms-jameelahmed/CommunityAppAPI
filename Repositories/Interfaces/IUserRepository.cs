using CommunityAppAPI.DTOs;
using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<AuthResponseDto> ValidateUserAsync(string email, string password);
    }
}
