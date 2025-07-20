using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IDbConnection _db;

        public ServiceRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<ServiceMaster>> GetAllAsync()
        {
            return await _db.QueryAsync<ServiceMaster>(
                "usp_Service_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ServiceMaster?> GetByIdAsync(long id)
        {
            return await _db.QueryFirstOrDefaultAsync<ServiceMaster>(
                "usp_Service_GetById",
                new { ServiceId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> CreateAsync(ServiceMaster service)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ServiceName", service.ServiceName);
            parameters.Add("Active", service.Active);
            parameters.Add("CreatedDate", service.CreatedDate);
            parameters.Add("CreatedBy", service.CreatedBy);

            return await _db.ExecuteScalarAsync<long>(
                "usp_Service_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UpdateAsync(ServiceMaster service)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ServiceId", service.ServiceId);
            parameters.Add("ServiceName", service.ServiceName);
            parameters.Add("Active", service.Active);
            parameters.Add("ModifiedDate", service.ModifiedDate);
            parameters.Add("ModifiedBy", service.ModifiedBy);

            var rows = await _db.ExecuteAsync(
                "usp_Service_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var rows = await _db.ExecuteAsync(
                "usp_Service_Delete",
                new { ServiceId = id },
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }
    }

}
