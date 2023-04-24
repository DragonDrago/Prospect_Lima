using Lima.Incomes.Api.Dto;
using RestEase;
using System.Threading.Tasks;

namespace Lima.Incomes.Api.Services
{
    public interface ITenantService
    {
        [AllowAnyStatusCode]
        [Post("tenants/{tenantId}/{userId}")]
        
        Task<TenantDto> GetTenantInfoAsync([Path] string tenantId, [Path] int userId);
    }
}
