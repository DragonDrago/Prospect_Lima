using Lima.Visits.Api.Dto;
using RestEase;
using System.Threading.Tasks;

namespace Lima.Visits.Api.Services
{
    public interface IOrderService
    {
        [AllowAnyStatusCode]
        [Post("orders/create")]
        Task<int?> CreateAsync([Body] NewOrderDto newOrderDto, [Header("Authorization")] string authorization);

        [AllowAnyStatusCode]
        [Post("orders/update")]
        Task<int> UpdateAsync([Body] NewOrderDto newOrderDto, [Header("Authorization")] string authorization);
    }
}
