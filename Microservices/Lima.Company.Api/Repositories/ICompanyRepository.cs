using Lima.Company.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Company.Api.Repositories
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Markup>> GetMarkupsAsync(string cuid);
        Task<IEnumerable<Module>> GetAvailableModulesAsync(string cuid);
        Task<IEnumerable<Role>> GetRolesAsync(string cuid);
        Task<License> GetLicenseAsync(string cuid);
    }
}
