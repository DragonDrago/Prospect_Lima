using AutoMapper;
using Lima.Core.Rest;
using Lima.Core.Tenant;
using Lima.Incomes.Api.Mapping;
using Lima.Incomes.Api.Model;
using Lima.Incomes.Api.Repositories;
using Lima.Incomes.Api.Services;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Incomes.Api
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
            JwtAuthenticationConfiguration jwtAuthenticationConfiguration = new JwtAuthenticationConfiguration();

            services.AddControllers();

            Configuration.Bind("JwtAuthentication", jwtAuthenticationConfiguration);
            string dbConnectionString = Configuration.GetConnectionString("SqlServerConnectionString");

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
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthenticationConfiguration.AccessTokenSecret)),
                    ValidIssuer = jwtAuthenticationConfiguration.Issuer,
                    ValidAudience = jwtAuthenticationConfiguration.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddMultiTenancy();
            services.AddTransient<IIncomesRepository>(provider => new IncomesRepository(provider.GetRequiredService<ITenantContext>()));
            services.AddAutoMapper(c => c.AddProfile(new DefaultMappingProfile()));

            //services.RegisterServiceForwarder<ITenantService>("tenant-service");
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddMultiTenancy<TenantHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMultiTenancy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
