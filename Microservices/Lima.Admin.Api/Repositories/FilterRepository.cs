using Dapper;
using Lima.Admin.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private string _connectionString;

        public FilterRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> EnableFilterAsync(int filterId, bool enabled, string cuid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: "dbo.updateFilter",
                    param: new { @filterId, @enabled, @cuid },
                    commandType: System.Data.CommandType.StoredProcedure) > 0;
            }
        }

        public async Task<IEnumerable<CompanyFilter>> GetFiltersAsync(string cuid)
        {
            List<CompanyFilter> filters = new List<CompanyFilter>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                await connection.QueryAsync<string, Filter, CompanyFilter>(
                    sql: "dbo.getFilters",
                    param: new { @cuid },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "FilterSectionName,FilterId",
                    map: (cf, f) =>
                    {
                        int sectionIndex = filters.FindIndex(f => f.SectionName == cf);

                        if (sectionIndex == -1)
                        {
                            filters.Add(new CompanyFilter()
                            {
                                SectionName = cf
                            });

                            sectionIndex = filters.Count - 1;
                        }

                        filters[sectionIndex].Filters.Add(f);

                        return filters[sectionIndex];
                    });

                return filters;
            }
        }
    }
}
