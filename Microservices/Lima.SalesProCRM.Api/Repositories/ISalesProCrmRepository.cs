using Lima.SalesProCRM.Api.Domains;
using Lima.SalesProCRM.Api.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.SalesProCRM.Api.Repositories
{
    public interface ISalesProCrmRepository
    {
        Task<SalesProCrmDomain> GetSalesById(int id);
        IEnumerable<SalesProCrmDomain> GetAllSales(SalesProCrmDomainRequest salesProDomain);
        Task<int> AddSales(SalesProCrmDomain salesProDomain);
        Task<int> UpdateSales(SalesProCrmDomain salesProDomain);
        Task DeleteSales(SalesProCrmDomain salesProDomain);
    }
}
