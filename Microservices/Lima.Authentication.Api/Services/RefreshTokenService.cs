using Lima.AuthenticationServer.Api.Model;
using Lima.Core;

namespace Lima.AuthenticationServer.Api.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenGeneratorService _tokenGenerator;

        public RefreshTokenService(ITokenGeneratorService tokenGenerator) =>
            (_tokenGenerator) = (tokenGenerator);

        public string Generate(User user) => _tokenGenerator.Generate(JwtAuthenticationConfiguration.REFRESH_TOKEN_SECRET,
            JwtAuthenticationConfiguration.ISSUER, JwtAuthenticationConfiguration.AUDIENCE,
            0);
    }
}
