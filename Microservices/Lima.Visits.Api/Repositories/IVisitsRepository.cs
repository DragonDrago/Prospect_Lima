using Lima.Visits.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Visits.Api.Repositories
{
    public interface IVisitsRepository
    {
        Task<IEnumerable<VisitInfo>> GetLastVisitsToDoctorsAsync(int offset, int userId);
        Task<IEnumerable<VisitInfo>> GetLastVisitsToPharmaciesAndDistributorsAsync(int offset, int userId);
        Task<IEnumerable<Drug>> GetDrugsFromVisitsToDoctorsAsync(int visitId);
        Task<IEnumerable<VisitInfo>> GetAllVisits(VisitFilter filter, int userId, int typeId);
        Task<IEnumerable<VisitInfo>> GetAllVisitsToDoctorsAsync(VisitFilter filter, int userId);
        Task<IEnumerable<VisitInfo>> GetAllVisitsToDistributorsAsync(VisitFilter visitFilter, int userId);
        Task<IEnumerable<VisitInfo>> GetAllVisitsToPharmaciesAsync(VisitFilter filter, int userId);
        Task<int> AddOrUpdateVisitToDoctorAsync(Visit newVisit, int userId);
        Task<int> AddOrUpdateVisitToDrugStore(Visit newVisit, int userId);
        Task<int> AddOrUpdateVisitToDistributorAsync(Visit newVisit, int userId);
        Task<bool> AddDistributorBalanceAsync(Visit newVisit, int userId);
        Task<bool> AddDrugStoreBalanceAsync(Visit newVisit, int userId);
        Task<IEnumerable<Balance>> GetDrugStoreBalanceAsync(int organizationId);
        Task<IEnumerable<Balance>> GetDistributorBalanceAsync(int organizationId);
        Task<DoctorStatistics> GetStatisticsOnVisitsToDoctorAsync(int userId); 
        Task<PharmacyStatistics> GetStatisticsOnVisitsToPharmacyAsync(int userId);
        Task<DistributorStatistics> GetStatisticsOnVisitsToDistributorAsync(int userId);
        Task CompleateVisitAsync(int visitId, double latitude, double longitude);
        Task<Visit> GetVisitByIdAsync(int visitId);
        Task<int> GetCountVisitsToDoctorsAsync(int userId);
    }
}
