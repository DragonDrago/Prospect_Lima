using Dapper;
using Lima.Core.Tenant;
using Lima.Reporting.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Reporting.Api.Repositories
{
    public class ReportingRepository : IReportingRepository
    {
        private ITenantContext _tenantContext;

        public ReportingRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<Report> GetCommonReport(ReportFilter reportFilter)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Report>(
                    sql: "dbo.getCommonReport",
                    param: new
                    {
                        @startDate = reportFilter.StartDate,
                        @endDate = reportFilter.EndDate,
                        @branchId = reportFilter.BranchId,
                        @userId = reportFilter.UserId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
