using Lima.Dictionaries.Api.Domain;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public interface IDoctorsRepository
    {
        Task<IEnumerable<Doctor>> FindAsync(string text);
        Task<IEnumerable<Doctor>> GetAll(DoctorFilter doctorFilter);
        Task<int> AddOrUpdate(Doctor doctor);
    }
}
