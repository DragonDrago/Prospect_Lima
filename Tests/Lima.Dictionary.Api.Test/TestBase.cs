using Lima.AuthenticationServer.Api.Services;
using Lima.Dictionaries.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Dictionary.Api.Test
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

            string jwtToken = tokenGeneratorService.Generate(config.AccessTokenSecret, config.Issuer, config.Audience, config.AccessTokenExpirationMinutes);

            HttpClient = webApplicationFactory.CreateClient();
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        }
    }
}
