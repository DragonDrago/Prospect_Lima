using Dapper;
using Lima.Company.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Company.Api.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private string _connectionString;

        public CompanyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Markup>> GetMarkupsAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Markup>(
                    sql: @"SELECT * FROM COMPANY_MARKUPS
	                        WHERE CompanyId = (SELECT Id FROM COMPANIES WHERE [UId] = @cuid)",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<IEnumerable<Module>> GetAvailableModulesAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Module>(
                    sql: "dbo.getAvailableModules",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Role>(
                    sql: "dbo.getRoles",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<License> GetLicenseAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<License>(sql: @"SELECT 
	                                                                c.Name AS CompanyName, 
	                                                                c.Inn, 
	                                                                c.LicenseDate, 
	                                                                u.EmployeeCount 
                                                                FROM COMPANIES c
                                                                LEFT JOIN 
                                                                (
	                                                                SELECT CompanyId, COUNT(*) EmployeeCount
	                                                                FROM USERS
	                                                                GROUP BY CompanyId
                                                                ) u ON u.CompanyId = c.Id
                                                                WHERE c.UId = @cuid",
                                                                param: new { @cuid });
            }
        }
    }
}
