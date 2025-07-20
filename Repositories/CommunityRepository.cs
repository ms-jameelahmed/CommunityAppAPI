using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly IDbConnection _db;

        public CommunityRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<CommunityMaster>> GetAllAsync()
        {
            return await _db.QueryAsync<CommunityMaster>(
                "usp_Community_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<CommunityMaster?> GetByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<CommunityMaster>(
                "usp_Community_GetById",
                new { CommunityId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CreateAsync(CommunityMaster community)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", community.Name);
            parameters.Add("Active", community.Active);
            parameters.Add("CreatedDate", community.CreatedDate);
            parameters.Add("CreatedBy", community.CreatedBy);

            // Output: newly inserted ID
            var result = await _db.ExecuteScalarAsync<int>(
                "usp_Community_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> UpdateAsync(CommunityMaster community)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CommunityId", community.CommunityId);
            parameters.Add("Name", community.Name);
            parameters.Add("Active", community.Active);
            parameters.Add("ModifiedDate", community.ModifiedDate);
            parameters.Add("ModifiedBy", community.ModifiedBy);

            var affected = await _db.ExecuteAsync(
                "usp_Community_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CommunityId", id);

            var affected = await _db.ExecuteAsync(
                "usp_Community_Delete",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }
    }


}
