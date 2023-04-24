using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Grants.Api.Repositories
{
    public class GrantsRepository : IGrantsRepository
    {
        private string _connectionString;

        public GrantsRepository(string connectionString)
        {
            _connectionString = connectionString;
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
    }
}
