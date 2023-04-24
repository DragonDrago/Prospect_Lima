using Dapper;
using Lima.Core.Tenant;
using Lima.Statistics.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Statistics.Api.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private ITenantContext _tenantContext;

        public StatisticsRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<SaleStatus> GetBestSalesPerMonthAsync()
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<SaleStatus>(
                    sql: "dbo.getBestSalesPerMonth",
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Sale>> GetSalesPerMonthAsync()
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Sale>(
                    sql: "dbo.getSalesPerMonth",
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Domain.Statistics> GetStatisticsAsync(string cuid, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Domain.Statistics>(
                    sql: "dbo.getStatistics",
                    param: new { @cuid, @userId },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<SaleStatus> GetWorstSalesPerMonthAsync()
        {
            return new SaleStatus();
        }
    }
}
