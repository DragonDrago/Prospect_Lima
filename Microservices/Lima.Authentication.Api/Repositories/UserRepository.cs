using Lima.AuthenticationServer.Api.Model;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;

namespace Lima.AuthenticationServer.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString) => _connectionString = connectionString;

        public async Task<User> FindAsync(string login)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                User user = null;

                await connection.QueryAsync<User, Role, User>(
                    sql: "dbo.getUser", 
                    param: new { login }, 
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id,RoleId",
                    map: (u, r) => 
                    {
                        if (user == null)
                            user = u;

                        if (!user.Roles.Exists(e => e.RoleId == r.RoleId))
                            user.Roles.Add(r);

                        return u;
                    });

                return user;
            }
        }

        public async Task<User> FindUserbyRefreshTokenAsync(string refreshToken)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<User>(
                    sql: "dbo.getUserByRefreshToken",
                    param: new { @refreshToken },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<string>> GetUserGrantsAsync(int userId, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<string>(
                    sql: "dbo.getGrants",
                    param: new { @userId, @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<bool> UpdateRefreshTokenAsync(User user, string refreshToken)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return (await connection.ExecuteAsync(
                    sql: "UPDATE USERS SET RefreshToken = @refreshToken WHERE Id = @userId",
                    param: new { @refreshToken, @userId = user.Id },
                    commandType: System.Data.CommandType.Text)) > 0;
            }
        }
    }
}
