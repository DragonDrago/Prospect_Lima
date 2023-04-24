using Lima.Core;
using Lima.Core.Exceptions;
using Lima.Core.Json;
using Lima.Core.Policy;
using Lima.Core.Rest;
using Lima.Core.Tenant;
using Lima.Dictionaries.Api.DatabaseContext;
using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.FileService;
using Lima.Dictionaries.Api.Mapping;
using Lima.Dictionaries.Api.Repositories;
using Lima.Dictionaries.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api
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

            services.AddAuthorization(config => 
            { 
                config.AddPolicy("DRUGS_ADD", policy => policy.Requirements.Add(new GrantRequirement("DRUGS_ADD")));
                config.AddPolicy("DRUGS_VIEW", policy => policy.Requirements.Add(new GrantRequirement("DRUGS_VIEW")));
                config.AddPolicy("DRUGS_EDIT", policy => policy.Requirements.Add(new GrantRequirement("DRUGS_EDIT")));
                config.AddPolicy("DRUGS_DELETE", policy => policy.Requirements.Add(new GrantRequirement("DRUGS_ADD")));
            });

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
                     //ValidateLifetime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAuthenticationConfiguration.ACCESS_TOKEN_SECRET)),
                     ValidIssuer = JwtAuthenticationConfiguration.ISSUER,
                     ValidAudience = JwtAuthenticationConfiguration.AUDIENCE,
                     ClockSkew = TimeSpan.Zero
                 };
            });
            services.AddGrantAccess();
            services.AddControllers().AddJsonOptions(configure =>
                  {
                      configure.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                  });
            services.AddMultiTenancy();
            services.AddSingleton<ICommonRepository>(new CommonRepository(dbConnectionString));
            services.AddSingleton<IDoctorsRepository>(new DoctorsRepository(dbConnectionString));
            services.AddSingleton<ILogger, Logger<Startup>>();
            services.AddTransient<IOrganizationsRepository>(x => new OrganizationsRepository(x.GetService<ITenantContext>(), dbConnectionString));
            services.AddTransient<IDrugsRepository, DrugsRepository>();
            //services.AddTransient<IProductsRepository, ProductsRepository>();
            //services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddAutoMapper(c => c.AddProfile(new DefaultMappingProfile()));

            services.RegisterServiceForwarder<IStaticContentService>("static-content-service");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            AppDbInitializer.Seed(app);
            app.UseGrantsAccess();
            
            //app.UseHttpsRedirection();
            

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
