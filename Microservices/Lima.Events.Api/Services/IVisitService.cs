using Lima.Events.Api.Dto;
using RestEase;
using System.Threading.Tasks;

namespace Lima.Events.Api.Services
{
    public interface IVisitService
    {
        [AllowAnyStatusCode]
        [Post("visits/pharmacies/add")]
        Task<int?> AddVisitToPharmacyAsync([Body] NewVisitDto newVisitDto, [Header("Authorization")] string authorization);

        [AllowAnyStatusCode]
        [Post("visits/distributors/add")]
        Task<int?> AddVisitToDistributorAsync([Body] NewVisitDto newVisitDto, [Header("Authorization")] string authorization);

        [AllowAnyStatusCode]
        [Post("visits/doctors/add")]
        Task<int?> AddVisitToDoctorAsync([Body] NewVisitDto newVisitDto, [Header("Authorization")] string authorization);
    }
}
