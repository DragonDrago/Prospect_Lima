using Lima.Core.Tenant;
using Lima.SalesProCRM.Api.Domains;
using Microsoft.EntityFrameworkCore;

namespace Lima.SalesProCRM.Api
{
    public class SalesProCrmDbContext : DbContext
    {
        public SalesProCrmDbContext()
        {

        }
        private readonly ITenantContext _tenantContext;

        public SalesProCrmDbContext(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesProCrmDomain>(entity => entity.ToTable("SALES_PROCRM"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_tenantContext.CurrentTenant.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<SalesProCrmDomain> SalesProCrmDomains { get; set; }
    }
}
