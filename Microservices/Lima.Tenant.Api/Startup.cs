using Lima.Core.Json;
using Lima.Tenant.Api.Domain;
using Lima.Tenant.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Tenant.Api
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
            //JwtAuthenticationConfiguration jwtAuthenticationConfiguration = new JwtAuthenticationConfiguration();

            services.AddControllers()
                .AddJsonOptions(confugure => confugure.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy());

#if DEBUG

            string dbConnectionString = Configuration.GetConnectionString("SqlServerConnectionStringDebug");
#endif
#if !DEBUG
            string dbConnectionString = Configuration.GetConnectionString("SqlServerConnectionString");
#endif
            //services.AddAuthentication(config =>
            //{
            //    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(config =>
            //{
            //    config.SaveToken = true;
            //    config.RequireHttpsMetadata = false;
            //    config.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthenticationConfiguration.AccessTokenSecret)),
            //        ValidIssuer = jwtAuthenticationConfiguration.Issuer,
            //        ValidAudience = jwtAuthenticationConfiguration.Audience,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            services.AddSingleton<ILogger, Logger<Startup>>();
            services.AddSingleton<ITenantRepository>(new TenantRepository(dbConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
