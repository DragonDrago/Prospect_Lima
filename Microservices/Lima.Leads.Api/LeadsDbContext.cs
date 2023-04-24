using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lima.Leads.Api.Domain;
using Lima.Core.Tenant;
using Microsoft.Extensions.Logging;

namespace Lima.Leads.Api
{
    public class LeadsDbContext:DbContext
    {
        private readonly ITenantContext _tenantContext;
        private readonly ILogger _logger;

        public LeadsDbContext(ITenantContext tenantContext, ILogger logger)
        {
            _tenantContext = tenantContext;
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeadsDomain>(e=>e.ToTable("LEADS"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _logger.LogInformation($"DbContextOptionsBuilder: {optionsBuilder}, ITenantContext: {_tenantContext?.CurrentTenant}");

            optionsBuilder.UseSqlServer(_tenantContext.CurrentTenant.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<LeadsDomain> LeadsDomains { get; set; }
    }
}
