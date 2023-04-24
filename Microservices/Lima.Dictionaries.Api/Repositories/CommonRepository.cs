using Dapper;
using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private string _connectionString;

        public CommonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Role>(sql: "SELECT Id, Name, [Desc] FROM USER_ROLES");
            }
        }

        public async Task<IEnumerable<EntityBase>> GetCountriesAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<EntityBase>(sql: "SELECT Id, Name FROM DICT_COUNTRIES");
            }
        }

        public async Task<IEnumerable<EntityBase>> GetRegionsAsync(int countryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<EntityBase>(sql: "SELECT Id, Name FROM DICT_REGIONS WHERE CountryId = @countryId", param: new { @countryId });
            }
        }

        public async Task<IEnumerable<EntityBase>> GetAreasAsync(int regionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<EntityBase>(sql: "SELECT Id, Name FROM DICT_AREAS WHERE RegionId = @regionId", param: new { @regionId });
            }
        }

        public async Task<IEnumerable<Producer>> GetProducersAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Producer>(sql: "SELECT p.Id, p.Name, c.Name AS CountryName, c.Id As CountryId FROM DICT_PRODUCERS p LEFT JOIN DICT_COUNTRIES c ON p.CountryId = c.Id");
            }
        }
    }
}
