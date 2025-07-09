using CommunityAppAPI.DTOs;
using CommunityAppAPI.Helpers;
using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;

namespace CommunityAppAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<AuthResponseDto> Authenticate(AuthRequest request)
        {
            var user = await _userRepository.ValidateUserAsync(request.Email, request.Password);
            if (user!=null)
            {
                var token = JwtHelper.GenerateToken(user.CustomerId, user.Email, _config);
                AuthResponseDto response = new AuthResponseDto();
                user.Token = token;            
            }
            return user;

        }
    }
}
