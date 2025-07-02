using CommunityAppAPI.DTOs;
using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto Authenticate(AuthRequest request);
    }
}
