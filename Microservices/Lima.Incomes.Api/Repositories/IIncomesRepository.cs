using Lima.Incomes.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Incomes.Api.Repositories
{
    public interface IIncomesRepository
    {
        Task<IEnumerable<IncomeSummary>> Summary(IncomeSummaryFilter incomeSummaryFilter);
        Task<int> AddOrUpdate(SaveIncome newIncome);
    }
}
