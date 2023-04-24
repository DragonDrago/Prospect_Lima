using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public interface IDrugsRepository
    {
        Task<string> GetDrugPhotoName(int drugId);
        Task<int> Count();
        Task<IEnumerable<Drug>> All(DrugFilter drugFilter);
        Task<Drug> FindById(int id);
        Task<int> AddOrUpdate(Drug drug);
        Task<bool> Remove(int id);
        Task<int> AddOrUpdateUnitAsync(EntityBase unit);
        Task<IEnumerable<EntityBase>> GetUnitsAsync();
        Task<IEnumerable<Drug>> FindAsync(string text);
        Task<IEnumerable<DrugSeries>> GetDrugSeriesAsync();
        Task<int> AddOrUpdateDrugSeriesAsync(DrugSeries drugSeries);
    }
}
