using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Statistics.Api.Repositories
{
    public interface IStatisticsRepository
    {
        Task<Domain.Statistics> GetStatisticsAsync(string cuid, int userId);
        Task<IEnumerable<Domain.Sale>> GetSalesPerMonthAsync();
        Task<Domain.SaleStatus> GetBestSalesPerMonthAsync();
        Task<Domain.SaleStatus> GetWorstSalesPerMonthAsync();
    }
}
