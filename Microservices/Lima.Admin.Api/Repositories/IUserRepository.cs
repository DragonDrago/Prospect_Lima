using Lima.Admin.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(string cuid);
        Task<int> AddOrUpdateUserAsync(User user, string cuid);
        Task<User> FindUserAsync(int id, string cuid);
    }
}
