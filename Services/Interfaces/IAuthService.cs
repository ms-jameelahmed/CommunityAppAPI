using CommunityAppAPI.DTOs;
using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Authenticate(AuthRequest request);
    }
}
