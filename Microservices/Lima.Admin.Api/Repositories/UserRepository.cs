using Dapper;
using Lima.Admin.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddOrUpdateUserAsync(User user, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: "dbo.insertOrUpdateUser",
                    param: new
                    {
                        @id = user.Id,
                        @fullName = user.FullName,
                        @phone = user.Phone,
                        @birthday = user.Birthday,
                        @address = user.Address,
                        @position = user.Position,
                        @login = user.Login,
                        @password = user.Password,
                        @roleId = user.RoleId,
                        @cuid = cuid
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<User> FindUserAsync(int id, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<User>(
                    sql: "dbo.getUserById",
                    param: new { @id, @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<User>(
                    sql: "dbo.getUsers",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
