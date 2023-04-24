using Lima.Incomes.Api.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lima.Gateway.Handlers
{
    public class TenantHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantService _tenantService;

        public TenantHandler(IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantService = tenantService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;

            if (userClaims != null)
            {
                var tenantClaim = userClaims.Claims.FirstOrDefault(f => f.Type == "tId");
                var userIdClaim = userClaims.Claims.FirstOrDefault(f => f.Type == "id");

                if (tenantClaim != null && userIdClaim != null)
                {
                    var tenantDto = await _tenantService.GetTenantInfoAsync(tenantClaim.Value, Convert.ToInt32(userIdClaim.Value));

                    if (tenantDto != null)
                    {
                        if (tenantDto.LicenseDate.Date < DateTime.Now.Date)
                        {
                            await SendLicenseExpiredMessage();
                            return new HttpResponseMessage(HttpStatusCode.BadRequest);
                        }

                        request.Headers.Add("xl-tenant-id", tenantDto.TenantId);
                        //request.Headers.Add("xl-company-name", tenantDto.CompanyName);
                        request.Headers.Add("xl-db-name", tenantDto.DbName);
                        request.Headers.Add("xl-db-connection", tenantDto.ConnectionString);
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task SendLicenseExpiredMessage()
        {
            _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
            _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new
            {
                Code = -2,
                Message = "The license date has expired!"
            });
        }
    }
}
