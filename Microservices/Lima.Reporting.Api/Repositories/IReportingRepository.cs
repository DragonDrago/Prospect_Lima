using Lima.Reporting.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Reporting.Api.Repositories
{
    public interface IReportingRepository
    {
        Task<Report> GetCommonReport(ReportFilter reportFilter);
    }
}
