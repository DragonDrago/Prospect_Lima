using Lima.AuthenticationServer.Api.Model;
using Lima.Core;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lima.AuthenticationServer.Api.Services
{
    public class TokenValidatorService : ITokenValidatorService
    {
        
        

        public bool Validate(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                //ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAuthenticationConfiguration.REFRESH_TOKEN_SECRET)),
                ValidIssuer = JwtAuthenticationConfiguration.ISSUER,
                ValidAudience = JwtAuthenticationConfiguration.AUDIENCE,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters,
                    out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
