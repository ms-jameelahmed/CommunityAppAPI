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

        public AuthResponseDto Authenticate(AuthRequest request)
        {
            var user = _userRepository.ValidateUser(request.Email, request.Password);
            if (user == null) return null;

            var token = JwtHelper.GenerateToken(user, _config);
            return new AuthResponseDto { Email = user.Email, Token = token };
        }
    }
}
