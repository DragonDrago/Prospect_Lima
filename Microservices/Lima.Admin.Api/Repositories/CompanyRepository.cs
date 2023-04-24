using Dapper;
using Lima.Admin.Api.Domain;
using Lima.Core.Tenant;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private string _connectionString;

        public CompanyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Company> GetCompanyAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Company>(
                    sql: @"SELECT * FROM COMPANIES
                            WHERE [Uid] = @cuid",
                    param: new { @cuid });
            }
        }

        public async Task<bool> SaveCompanyAsync(Company company, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: @"SELECT * FROM COMPANIES
                            WHERE [Uid] = @cuid",
                    param: new { @cuid }) > 0;
            }
        }

        public async Task<IEnumerable<Branch>> GetBranchesAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Branch>(
                    sql: @"dbo.getBranches",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<bool> AddOrUpdateBranchAsync(Branch branch, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: @"dbo.insertOrUpdateBranch",
                    param: new 
                    { 
                        @id = branch.Id,
                        @cuid = cuid,
                        @chiefUserId = branch.ChiefUserId,
                        @name = branch.Name.Trim(),
                        @address = branch.Address
                    },
                    commandType: System.Data.CommandType.StoredProcedure) > 0;
            }
        }

        public async Task<IEnumerable<Department>> GetDepartmentAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Department>(
                    sql: @"dbo.getDepartments",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<int> AddOrUpdateDepartmentAsync(Department department, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: @"dbo.insertOrUpdateDepartment",
                    param: new
                    {
                        @id = department.Id,
                        @cuid = cuid,
                        @name = department.Name.Trim(),
                        @branchId = department.BranchId,
                        @chiefUserId = department.ChiefUserId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
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

        public async Task<int> AddOrUpdateMarkupAsync(Markup markup, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: @"dbo.insertOrUpdateMarkup",
                    param: new 
                    { 
                        @id = markup.Id,
                        @name = markup.Name?.Trim(),
                        @cuid = cuid,
                        @percent = markup.Percent,
                        @markupPercent = markup.MarkupPercent
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Role>(sql: "dbo.getRoles", param: new { @cuid }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Entity>> GetStoragesAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Entity>(sql: @"SELECT Id, Name FROM DICT_STORAGES
                                                                    WHERE CompanyId = (SELECT Id FROM COMPANIES WHERE [UId] = @cuid)", 
                                                            param: new { @cuid }, 
                                                            commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<int> AddOrUpdateStorage(Entity storage, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: @"dbo.insertOrUpdateMarkup",
                    param: new
                    {
                        @id = storage.Id,
                        @name = storage.Name.Trim(),
                        @cuid = cuid
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Entity>> GetUnits(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Entity>(sql: @"SELECT Id, Name FROM DICT_UNITS
                                                                    WHERE CompanyId = (SELECT Id FROM COMPANIES WHERE [UId] = @cuid) OR CompanyId IS NULL",
                                                            param: new { @cuid },
                                                            commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<int> AddOrUpdateUnit(Entity unit, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: @"dbo.insertOrUpdateUnit",
                    param: new
                    {
                        @id = unit.Id,
                        @name = unit.Name.Trim(),
                        @cuid = cuid
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
