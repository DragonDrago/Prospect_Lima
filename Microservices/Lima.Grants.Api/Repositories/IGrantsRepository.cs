using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Grants.Api.Repositories
{
    public interface IGrantsRepository
    {
        Task<IEnumerable<string>> GetUserGrantsAsync(int userId, string cuid);
    }
}
