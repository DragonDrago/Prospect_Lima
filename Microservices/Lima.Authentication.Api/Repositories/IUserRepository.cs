using Lima.AuthenticationServer.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.AuthenticationServer.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindAsync(string login);
        Task<bool> UpdateRefreshTokenAsync(User user, string refreshToken);
        Task<User> FindUserbyRefreshTokenAsync(string refreshtoken);
        Task<IEnumerable<string>> GetUserGrantsAsync(int userId, string cuid);
    }
}
