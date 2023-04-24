using Lima.SalesProCRM.Api.Domains;
using Lima.SalesProCRM.Api.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.SalesProCRM.Api.Repositories
{
    public class SalesProCrmRepository : ISalesProCrmRepository
    {
        private readonly SalesProCrmDbContext salesProCrmDbContext;

        public SalesProCrmRepository(SalesProCrmDbContext salesProCrmDbContext)
        {
            this.salesProCrmDbContext = salesProCrmDbContext;
        }
        public async Task<int> AddSales(SalesProCrmDomain salesProDomain)
        {
            await salesProCrmDbContext.SalesProCrmDomains.AddAsync(salesProDomain);
            await salesProCrmDbContext.SaveChangesAsync();
            return salesProDomain.Id;
        }

        public async Task DeleteSales(SalesProCrmDomain salesProDomain)
        {
            var entity = salesProCrmDbContext.SalesProCrmDomains.Find(salesProDomain.Id);
            salesProCrmDbContext.SalesProCrmDomains.Remove(entity);
            await salesProCrmDbContext.SaveChangesAsync();
        }

        public IEnumerable<SalesProCrmDomain> GetAllSales(SalesProCrmDomainRequest salesProDomain)
        {
            var entities = salesProCrmDbContext.SalesProCrmDomains.AsQueryable();
            if(salesProDomain != null)
            {
                if (salesProDomain.Id > 0)
                {
                    entities = entities.Where(w => w.Id == salesProDomain.Id);
                }
                if (!string.IsNullOrEmpty(salesProDomain.LdiName?.Trim()))
                {
                    string fullName = salesProDomain.LdiName.ToLower().Trim();
                    entities = entities.Where(w => w.LdiName.ToLower().Contains(fullName));
                }
                if (!string.IsNullOrEmpty(salesProDomain.LdiStatus?.Trim()))
                {
                    string attachedTo = salesProDomain.LdiStatus.ToLower().Trim();
                    entities = entities.Where(w => w.LdiStatus.ToLower().Contains(attachedTo));
                }
                if (!string.IsNullOrEmpty(salesProDomain.TotalSum.ToString()?.Trim()))
                {
                    string jobTitle = salesProDomain.TotalSum.ToString().ToLower().Trim();
                    entities = entities.Where(w => w.TotalSum.ToString().ToLower().Contains(jobTitle));
                }
                if (!string.IsNullOrEmpty(salesProDomain.DateTime.ToString()?.Trim()))
                {
                    string status = salesProDomain.DateTime.ToString();
                    entities = entities.Where(w => w.DateTime.ToString().Contains(status));
                }
            }
            return entities.ToList();
        }

        public async Task<SalesProCrmDomain> GetSalesById(int id)
        {
             var entity = await salesProCrmDbContext.SalesProCrmDomains.FindAsync(id);
            return entity;
        }

        public async Task<int> UpdateSales(SalesProCrmDomain salesProDomain)
        {
            var entityInDb = await salesProCrmDbContext.SalesProCrmDomains.FindAsync(salesProDomain.Id);
            if(entityInDb != null)
            {
                entityInDb.Check = salesProDomain.Check;
                entityInDb.ProductsId = salesProDomain.ProductsId;
                entityInDb.DateTime = salesProDomain.DateTime;
                entityInDb.TotalSum = salesProDomain.TotalSum;
                entityInDb.LdiStatus = salesProDomain.LdiStatus;
                entityInDb.LdiId = salesProDomain.LdiId;
                entityInDb.LdiAttachedTo = salesProDomain.LdiAttachedTo;
            }
            await salesProCrmDbContext.SaveChangesAsync();
            return salesProDomain.Id;
        }
    }
}
