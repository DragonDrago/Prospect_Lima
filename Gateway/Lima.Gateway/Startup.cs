using Lima.Core;
using Lima.Core.Json;
using Lima.Core.Rest;
using Lima.Gateway.CustomUserClaims;
using Lima.Gateway.Handlers;
using Lima.Gateway.Services;
using Lima.Incomes.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnectionString = Configuration.GetConnectionString("SqlServerConnectionString");

            services.AddControllers()
                .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy());

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAuthenticationConfiguration.ACCESS_TOKEN_SECRET)),
                    ValidIssuer = JwtAuthenticationConfiguration.ISSUER,
                    ValidAudience = JwtAuthenticationConfiguration.AUDIENCE,
                    ClockSkew = TimeSpan.Zero
                };

                //config.Events = new JwtBearerEvents()
                //{
                //    OnTokenValidated = async context =>
                //    {
                //        var user = context.Principal;
                //        var identity = (ClaimsIdentity)user.Identity;

                //        var grantsService = context.HttpContext.RequestServices.GetService<IGrantsService>();
                //        var logger = context.HttpContext.RequestServices.GetService<ILogger>();

                //        int userId = Convert.ToInt32(user.Claims.FirstOrDefault(f => f.Type == "id").Value);
                //        string cuid = user.Claims.FirstOrDefault(f => f.Type == "cuid").Value;

                //        var grants = await grantsService.GetGrantsAsync(cuid, userId);

                //        logger.LogDebug($"Grants count: {grants?.Count()}, Grants: {string.Join(", ", grants)}");

                //        foreach (var grant in grants)
                //        {
                //            identity.AddClaim(new Claim("grants", grant));
                //        }
                //    }
                //};
            });

            services.AddSingleton<ILogger, Logger<Startup>>();

            services.RegisterServiceForwarder<ITenantService>("tenant-service");
            services.RegisterServiceForwarder<IGrantsService>("grants-service");

            services.AddOcelot()
                .AddDelegatingHandler<TenantHandler>(true)
                .AddDelegatingHandler<GrantsHandler>(true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lima.Gateway", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowAnyOrigin());
            

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseOcelot().Wait();
        }
    }
}
