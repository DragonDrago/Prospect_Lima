using Lima.Admin.Api;
using Lima.AuthenticationServer.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Test
{
    public abstract class TestBase
    {
        protected HttpClient HttpClient { get; set; }

        public TestBase()
        {
            CreateClient();
        }

        private void CreateClient()
        {
            WebApplicationFactory<Startup> webApplicationFactory = new WebApplicationFactory<Startup>();

            JwtConfig config = new JwtConfig();
            TokenGeneratorService tokenGeneratorService = new TokenGeneratorService();

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", $"2"),
                new Claim("tId", $"9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08"),
                new Claim("cuid", "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08")
            };

            string jwtToken = tokenGeneratorService.Generate(
                config.AccessTokenSecret, 
                config.Issuer, 
                config.Audience, 
                config.AccessTokenExpirationMinutes,
                claims);

            HttpClient = webApplicationFactory.CreateClient();
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        }
    }
}
