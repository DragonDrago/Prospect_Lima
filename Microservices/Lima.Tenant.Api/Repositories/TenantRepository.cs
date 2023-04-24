using Dapper;
using Lima.Tenant.Api.Domain;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Tenant.Api.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private string _connectionString;

        public TenantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<TenantInfo> GetTenantInfo(string tenantId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QuerySingleOrDefaultAsync<TenantInfo>(
                    sql: @"SELECT Name AS CompanyName, DbName, TenantId, LicenseDate 
                            FROM COMPANIES c
                            LEFT JOIN USERS u ON u.CompanyId = c.Id
                          WHERE c.TenantId = @tenantId AND u.Id = @userId",
                    param: new { @tenantId, userId });
            }
        }
    }
}
