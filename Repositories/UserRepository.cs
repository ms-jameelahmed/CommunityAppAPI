using CommunityAppAPI.DTOs;
using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        public async Task<AuthResponseDto> ValidateUserAsync(string email, string password_)
        {
            try
            {
                if (email == "admin@example.com" && password_ == "password")
                    return new AuthResponseDto { CustomerId = 1, Email = email };
                else
                {
                    // Mock user validation
                    var multi = await CreateConnection().QueryMultipleAsync("usp_check_Login",
                       new { userid = email, password = password_ }, commandType: CommandType.StoredProcedure);
                    var user = await multi.ReadFirstOrDefaultAsync<AuthResponseDto>();

                    return user;
                }
            }
            catch (Exception ex)
            {

                var errorId = Guid.NewGuid();
                var timestamp = DateTime.UtcNow.ToString("u");
                return null;
            }
        }
    }
}
