using Lima.Core;
using Lima.Core.Exceptions;
using Lima.Core.Json;
using Lima.Core.Rest;
using Lima.Core.Tenant;
using Lima.Events.Api.Mapping;
using Lima.Events.Api.Repositories;
using Lima.Events.Api.Services;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Events.Api
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => 
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        //ValidateLifetime = true,
                        RequireExpirationTime = false,
                        ValidIssuer = JwtAuthenticationConfiguration.ISSUER,
                        ValidAudience = JwtAuthenticationConfiguration.AUDIENCE,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAuthenticationConfiguration.ACCESS_TOKEN_SECRET)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddControllers()
                .AddJsonOptions(comfigure => 
                {
                    comfigure.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                });

            services.AddAutoMapper(x => x.AddProfile<DefaultMappingProfile>());
            services.AddMultiTenancy();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILogger, Logger<Startup>>();
            services.AddTransient<ClaimsPrincipal>(x => x.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.RegisterServiceForwarder<IVisitService>("visit-service");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
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
