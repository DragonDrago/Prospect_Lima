using Lima.Orders.Api.Domain;
using Lima.Orders.Api.Dto;
using RestEase;
using System.Threading.Tasks;

namespace Lima.Orders.Api.Services
{
    public interface ISalesService
    {
        //[AllowAnyStatusCode]
        [Post("sales/create")]
        Task<SaleResult> CreateSaleAsync([Body] NewSaleDto newSaleDto, [Header("Authorization")] string authorization);
    }
}
