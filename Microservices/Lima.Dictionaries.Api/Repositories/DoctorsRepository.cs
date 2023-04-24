using Dapper;
using Lima.Dictionaries.Api.DatabaseContext;
using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public class DoctorsRepository : IDoctorsRepository
    {
        private string _connectionString;
        private DictionaryDbContext _dictionaryDbContext;

        public DoctorsRepository(string connectionString)
        {
            _connectionString = connectionString;
            //_dictionaryDbContext = dictionaryDbContext;
        }

        public async Task<IEnumerable<Doctor>> FindAsync(string text)
        {
            if (text == null)
                return Enumerable.Empty<Doctor>();

            string translitedText = TextUtils.SmartConvert(text);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Doctor>(
                    sql: "dbo.getDoctorsByName",
                    param: new { @text, @translitText = translitedText },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<int> AddOrUpdate(Doctor doctor)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteScalarAsync<int>(
                    sql: "dbo.insertOrUpdateDoctor",
                    param: new 
                    { 
                        @id = doctor.Id,
                        @fullName = doctor.FullName,
                        @comment = doctor.Comment,
                        @phone = doctor.Phone,
                        @position = doctor.Position
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Doctor>> GetAll(DoctorFilter doctorFilter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                //List<Doctor> doctors = new List<Doctor>();

                var doctors = await connection.QueryAsync<Doctor>(
                    sql: "dbo.getDoctors",
                    //splitOn: "separate",
                    commandType: System.Data.CommandType.StoredProcedure
                    //map: (d, o) => 
                    //{ 
                    //    int index = doctors.FindIndex(f => f.Id == d.Id);

                    //    if (index == -1)
                    //    {
                    //        doctors.Add(d);
                    //        index = doctors.Count - 1;
                    //    }

                    //    if (o != null)
                    //        doctors[index].Organizations.Add(o);

                    //    return d;
                    //}
                    );


                if (doctorFilter.DoctorId > 0)
                {
                    doctors = doctors.Where(w => w.Id == doctorFilter.DoctorId).ToList();
                }

                if (!string.IsNullOrEmpty(doctorFilter.Phone))
                {
                    doctors = doctors.Where(w => w.Phone == doctorFilter.Phone).ToList();
                }

                if (!string.IsNullOrEmpty(doctorFilter.DoctorName?.Trim()))
                {
                    string doctorName = doctorFilter.DoctorName.Trim().ToLower();

                    doctors = doctors.Where(w => w.FullName.ToLower().Contains(doctorName)).ToList();
                }

                return doctors;
            }
        }
    }
}
