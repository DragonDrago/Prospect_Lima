using Lima.Tenant.Api.Domain;
using System.Threading.Tasks;

namespace Lima.Tenant.Api.Repositories
{
    public interface ITenantRepository
    {
        Task<TenantInfo> GetTenantInfo(string tenantId, int userId);
    }
}
