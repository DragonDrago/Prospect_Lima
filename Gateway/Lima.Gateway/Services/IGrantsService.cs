using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Gateway.Services
{
    public interface IGrantsService
    {
        [AllowAnyStatusCode]
        [Get("grants/get/{userId}/{cuid}")]
        Task<IEnumerable<string>> GetGrantsAsync([Path] string cuid, [Path] int userId);
    }
}
