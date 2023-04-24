using Lima.Orders.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Orders.Api.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderSummary>> GetSummary(OrderSummaryFilter orderSummaryFilter);
        Task<int> CreateOrUpdateAsync(NewOrder newOrder);
        Task<IEnumerable<Order>> All(OrderFilter orderFilter, int userId);
        Task<Statistics> GetOrdersStatisticsAsync(int userId);
        Task<bool> ChangeOrderStatusAsync(int orderId, int status);
        Task<Order> GetOrderAsync(string orderIdOrCheck);
    }
}
