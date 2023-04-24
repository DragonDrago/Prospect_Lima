using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lima.AuthenticationServer.Api.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public string Generate(string secretKey, string issuer, string audience, double expires, IEnumerable<Claim> claims = null)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                null,//DateTime.UtcNow,
                null,//DateTime.UtcNow.AddMinutes(expires),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
