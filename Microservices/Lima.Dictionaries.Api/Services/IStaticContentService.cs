using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.Dto;
using RestEase;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Services
{
    public interface IStaticContentService
    {
        [AllowAnyStatusCode]
        [Post("static-content/save-drug-photo")]
        Task<FObject> SaveDrugPhotoAsync([Body] SavePhotoDto newOrderDto, [Header("Authorization")] string authorization);

        [AllowAnyStatusCode]
        [Post("static-content/remove-drug-photo")]
        Task<bool> RemoveDrugPhotoAsync(string photoName);
    }
}
