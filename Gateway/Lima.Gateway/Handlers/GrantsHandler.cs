using Lima.Gateway.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lima.Gateway.Handlers
{
    public class GrantsHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGrantsService _grantsService;

        public GrantsHandler(IHttpContextAccessor httpContextAccessor, IGrantsService grantsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _grantsService = grantsService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var userClaims = _httpContextAccessor.HttpContext.User;

            if (userClaims != null)
            {
                var cuidClaim = userClaims.Claims.FirstOrDefault(f => f.Type == "cuid");
                var userIdClaim = userClaims.Claims.FirstOrDefault(f => f.Type == "id");

                if (cuidClaim != null && userIdClaim != null)
                {
                    var grants = await _grantsService.GetGrantsAsync(cuidClaim.Value, Convert.ToInt32(userIdClaim.Value));

                    if (grants != null)
                    {
                        request.Headers.Add("grants", string.Join(",", grants));
                        
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
