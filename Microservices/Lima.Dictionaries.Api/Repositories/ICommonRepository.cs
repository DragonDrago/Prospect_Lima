using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public interface ICommonRepository
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<IEnumerable<EntityBase>> GetCountriesAsync();
        Task<IEnumerable<EntityBase>> GetRegionsAsync(int countryId);
        Task<IEnumerable<EntityBase>> GetAreasAsync(int regionId);
        Task<IEnumerable<Producer>> GetProducersAsync();
    }
}
