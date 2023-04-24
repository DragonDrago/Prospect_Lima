using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public interface IOrganizationsRepository
    {
        Task<IEnumerable<Organization>> FindAsync(string query, int typeId);
        Task<Organization> GetByIdAsync(int id);
        Task<int> AddOrUpdateOrganizationAsync(Organization organization, List<int> doctorsIds = null);
        Task<Response> GetDistributorsAsync(DistributorFilter distributorFilter);
        Task<Response> GetDrugStoresAsync(PharmacyFilter pharmacyFilter);
        Task<Response> GetHealthCareFacilityAsync(HealthCareFacilityFilter healthCareFacilityFilter);
        Task<IEnumerable<EntityBase>> GetHealthCareFacilityTypesAsync();
    }
}
