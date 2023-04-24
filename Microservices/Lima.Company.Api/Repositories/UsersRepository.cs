using Dapper;
using Lima.Company.Api.Domain;
using Lima.Core.Tenant;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Company.Api.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<User>> FindUserByNameAsync(string userName, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<User>(
                    sql: @"SELECT u.Id, u.FullName FROM USERS u
                            LEFT JOIN COMPANIES c ON c.Id = u.CompanyId
                        WHERE FullName LIKE '%'+@userName+'%' AND c.UId = @cuid",
                    param: new { @userName, @cuid });
            }
        }

        public async Task<Profile> GetProfile(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Profile>(sql: @"SELECT u.[Id]
                                                                      ,u.[Email]
                                                                      ,u.[Phone]
                                                                      ,u.[FullName]
                                                                      ,u.[Birthday]
                                                                      ,u.[Position]
                                                                      ,u.[Address]
                                                                      ,ur.Name AS RoleName
	                                                                  ,d.Name AS Department
	                                                                  ,b.Name AS RegionName
	  
                                                                  FROM USERS u
                                                                  LEFT JOIN USER_ROLES ur ON u.RoleId = ur.Id
                                                                  LEFT JOIN DEPARTMENTS d ON d.Id = u.DepartmentId
                                                                  LEFT JOIN BRANCHES b ON b.Id = d.BranchId
                                                                  WHERE u.Id = @id;", param: new { @id = userId});
            }
        }
    }
}
