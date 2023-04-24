using Lima.Admin.Api.Mapping;
using Lima.Admin.Api.Repositories;
using Lima.Core;
using Lima.Core.Exceptions;
using Lima.Core.Json;
using Lima.Core.Policy;
using Lima.Core.Tenant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Admin.Api
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
            string dbConnectionString = Configuration.GetConnectionString("LimaCommonConnectionString");

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
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
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("SETTINGS_VIEW", policy => policy.Requirements.Add(new GrantRequirement("SETTINGS_VIEW")));
            });
            services.AddControllers().AddJsonOptions(configure =>
                    {
                        configure.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                    });
            services.AddMultiTenancy();
            services.AddAutoMapper(c => c.AddProfile(new DefaultMappingProfile()));
            
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddSingleton<ILogger, Logger<Startup>>();
            services.AddSingleton<ICompanyRepository>(new CompanyRepository(dbConnectionString));
            services.AddSingleton<IFilterRepository>(new FilterRepository(dbConnectionString));
            services.AddSingleton<IUserRepository>(new UserRepository(dbConnectionString));
            services.AddGrantAccess();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGrantsAccess();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMultiTenancy();
            app.UseLimaExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
