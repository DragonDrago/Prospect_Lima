using Lima.Tenant.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lima.Tenant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private ITenantRepository _tenantRepository;

        public TenantsController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        [HttpPost("{tenantId}/{userId}")]
        //[Authorize]
        public async Task<IActionResult> Index(string tenantId, int userId)
        {
            var tenant = await _tenantRepository.GetTenantInfo(tenantId, userId);

# if DEBUG
            tenant.ConnectionString = $"Data Source=localhost;Initial Catalog=LIMA_TEST;Connect Timeout=30;User id=lima;;Min Pool Size=1;Max pool size=10;";
#endif

#if !DEBUG
            tenant.ConnectionString = $"Data Source=192.168.88.250;Initial Catalog={tenant.DbName};Connect Timeout=30;User id=lima;Password=lima;Min Pool Size=1;Max pool size=10;";
#endif
            return Ok(tenant);
        }
           
    }
}
