using Lima.Core.Tenant;
using Lima.Core;
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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lima.Core.Json;
using Microsoft.EntityFrameworkCore;
using Lima.Core.Exceptions;
using Lima.Leads.Api.Repository;

namespace Lima.Leads.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(config =>
              {
                  config.SaveToken = true;
                  config.RequireHttpsMetadata = false;
                  config.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateIssuerSigningKey = true,
                      ValidateAudience = true,
                      ValidateIssuer = true,
                      RequireExpirationTime = false,
                      ValidateLifetime = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAuthenticationConfiguration.ACCESS_TOKEN_SECRET)),
                      ValidIssuer = JwtAuthenticationConfiguration.ISSUER,
                      ValidAudience = JwtAuthenticationConfiguration.AUDIENCE,
                      ClockSkew = TimeSpan.Zero
                  };
              });

            
            services.AddControllers()
              .AddJsonOptions(configure =>
              {
                  configure.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
              });

            services.AddMultiTenancy();
            services.AddSingleton<ILogger, Logger<Startup>>();
            services.AddDbContext<LeadsDbContext>();
            services.AddScoped<ILeadsRepository, LeadsRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lima.Leads.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lima.Leads.Api v1"));
            }
            AppDbInitializer.Seed(app);
            app.UseMultiTenancy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

           

            app.UseLimaExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
