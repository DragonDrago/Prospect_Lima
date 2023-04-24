using Lima.Sales.Api.Domain;
using RestEase;
using System.Threading.Tasks;

namespace Lima.Sales.Api.Services
{
    public interface IOrderService
    {
        [Get("orders/find/{checkOrId}")]
        Task<Order> GetOrderAsync([Path] string checkOrId, [Header("Authorization")] string authorization);
    }
}
