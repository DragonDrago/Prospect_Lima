using Lima.Admin.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Repositories
{
    public interface IFilterRepository
    {
        Task<IEnumerable<CompanyFilter>> GetFiltersAsync(string cuid);
        Task<bool> EnableFilterAsync(int filterId, bool enable, string cuid);
    }
}
