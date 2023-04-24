using System.Collections.Generic;
using System.Security.Claims;

namespace Lima.AuthenticationServer.Api.Services
{
    public interface ITokenGeneratorService
    {
        string Generate(string secretKey, string issuer, string audience, double expires, IEnumerable<Claim> claims = null);
    }
}
