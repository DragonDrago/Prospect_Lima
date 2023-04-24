using Lima.Admin.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyAsync(string cuid);
        Task<bool> SaveCompanyAsync(Company company, string cuid);
        Task<IEnumerable<Branch>> GetBranchesAsync(string cuid);
        Task<bool> AddOrUpdateBranchAsync(Branch branch, string cuid);
        Task<IEnumerable<Department>> GetDepartmentAsync(string cuid);
        Task<int> AddOrUpdateDepartmentAsync (Department department, string cuid);
        Task<IEnumerable<Markup>> GetMarkupsAsync(string cuid);
        Task<int> AddOrUpdateMarkupAsync(Markup markup, string cuid);
        Task<IEnumerable<Role>> GetRolesAsync(string cuid);
        Task<IEnumerable<Entity>> GetStoragesAsync(string cuid);
        Task<int> AddOrUpdateStorage(Entity storage, string cuid);
        Task<int> AddOrUpdateUnit(Entity unit, string cuid);
        Task<IEnumerable<Entity>> GetUnits(string cuid);
    }
}
