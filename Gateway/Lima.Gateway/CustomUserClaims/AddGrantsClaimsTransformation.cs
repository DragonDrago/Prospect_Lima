using Lima.Gateway.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Gateway.CustomUserClaims
{
    public class AddGrantsClaimsTransformation : IClaimsTransformation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGrantsService _grantsService;

        public AddGrantsClaimsTransformation(IHttpContextAccessor httpContextAccessor, IGrantsService grantsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _grantsService = grantsService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            var cuidClaim = clone.Claims.FirstOrDefault(f => f.Type == "cuid");
            var userIdClaim = clone.Claims.FirstOrDefault(f => f.Type == "id");

            if (cuidClaim != null && userIdClaim != null)
            {
                var grants = await _grantsService.GetGrantsAsync(cuidClaim.Value, Convert.ToInt32(userIdClaim.Value));

                if (grants != null)
                {
                    // Add role claims to cloned identity
                    foreach (var grant in grants)
                    {
                        var claim = new Claim("grants", grant);
                        newIdentity.AddClaim(claim);
                    }
                }
            }

            return clone;
        }
    }
}
