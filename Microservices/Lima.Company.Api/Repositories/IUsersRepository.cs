using Lima.Company.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Company.Api.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> FindUserByNameAsync(string userName, string cuid);
        Task<Profile> GetProfile(int userId); 
    }
}
