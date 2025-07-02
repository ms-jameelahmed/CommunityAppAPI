using CommunityAppAPI.Models;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var result = _authService.Authenticate(request);
            if (result == null)
            {
                _logger.LogWarning("Unauthorized login attempt for {Email}", request.Email);
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
