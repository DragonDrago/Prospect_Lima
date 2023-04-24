using Lima.AuthenticationServer.Api.Model;
using Lima.Core;
using System.Collections.Generic;
using System.Security.Claims;

namespace Lima.AuthenticationServer.Api.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private ITokenGeneratorService _tokenGenerator;

        public AccessTokenService(ITokenGeneratorService tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public string Generate(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", $"{user.Id}"),
                new Claim("tId", $"{user.TenantId}"),
                new Claim("cuid", user.CompanyUId)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            //foreach (var grant in user.Grants)
            //{
            //    claims.Add(new Claim("grants", grant));
            //}

            return _tokenGenerator.Generate(
                JwtAuthenticationConfiguration.ACCESS_TOKEN_SECRET,
                JwtAuthenticationConfiguration.ISSUER,
                JwtAuthenticationConfiguration.AUDIENCE,
                JwtAuthenticationConfiguration.ACCESS_TOKEN_EXPIRATION_MINUTES,
                claims);
        }  
    }
}
