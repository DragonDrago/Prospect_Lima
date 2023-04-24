using Lima.Core.Tenant;
using Lima.Orders.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System;
using System.Data;
using System.Linq;

namespace Lima.Orders.Api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ITenantContext _tenantContext;

        public OrderRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<IEnumerable<Order>> All(OrderFilter orderFilter, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<Order> orders = new List<Order>();

                await connection.QueryAsync<Order, Organization, OrderDetailings, Order>(
                    sql: "dbo.getOrders",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Id,OrganizationId,Id",
                    map: (o, org, od) =>
                    {
                        int index = orders.FindIndex(f => f.Id == o.Id);

                        if (index == -1)
                        {
                            if (org.OrganizationTypeId == 1)
                                o.Organization = org;
                            else if (org.OrganizationTypeId == 3)
                                o.Distributor = org;

                            orders.Add(o);

                            index = orders.Count - 1;
                        }

                        if (od != null)
                        {
                            orders[index].OrderDetailings.Add(od);
                        }
                        
                        return o;
                    });

                if (orderFilter != null)
                {
                    if (orderFilter.OrganizationTypeId == 1)
                        orders = orders.Where(w => w.Organization != null).ToList();

                    if(orderFilter.OrganizationTypeId == 3)
                        orders = orders.Where(w => w.Distributor != null).ToList();

                    if (orderFilter.OrderId > 0)
                        orders = orders.Where(w => w.Id == orderFilter.OrderId).ToList();

                    if (orderFilter.UserId > 0)
                        orders = orders.Where(w => w.UserId == orderFilter.UserId).ToList();

                    if (orderFilter.Dates != null && orderFilter.Dates.Length > 0)
                        orders = orders.Where(w => w.DateCreate >= orderFilter.Dates.First() && w.DateCreate <= orderFilter.Dates.Last()).ToList();
                }

                return orders;
            }
        }

        public async Task<bool> ChangeOrderStatusAsync(int orderId, int status)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: "UPDATE ORDERS SET StatusId = @statusId WHERE Id = @orderId",
                    param: new { @orderId, @statusId = status }) > 0;
            }
        }

        public async Task<int> CreateOrUpdateAsync(NewOrder newOrder)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int orderId = await connection.QueryFirstOrDefaultAsync<int>(
                          sql: "dbo.insertOrUpdateOrder",
                          param: new 
                          { 
                              @id = newOrder.Id, 
                              @visitId = newOrder.VisitId, 
                              @statusId = newOrder.StatusId,
                              @name = newOrder.Name, 
                              @prepaymentPercent = newOrder.PrepaymentPercent 
                          },
                          commandType: System.Data.CommandType.StoredProcedure,
                          transaction: transaction);

                        DataTable orderDetailingDataTable = new DataTable("ORDER_DETAILING");
                        orderDetailingDataTable.Columns.Add("DrugId");
                        orderDetailingDataTable.Columns.Add("Amount");

                        newOrder.OrdersDetailing.ForEach(f => orderDetailingDataTable.Rows.Add(f.DrugId, f.Amount));

                        await connection.ExecuteAsync(
                            sql: "dbo.insertOrUpdateOrderDetailings",
                            param: new { @orderId, @orderDetailing = orderDetailingDataTable.AsTableValuedParameter() },
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        await transaction.CommitAsync();

                        return orderId;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<Order> GetOrderAsync(string orderIdOrCheck)
        {
            int orderId;

            int.TryParse(orderIdOrCheck, out orderId);

            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                Order order = null;

                await connection.QueryAsync<Order, Organization, OrderDetailings, Order>(
                    sql: "dbo.getOrder",
                    param: new { @check = orderIdOrCheck, @orderId },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Id,OrganizationId,Id",
                    map: (o, org, od) => 
                    {
                        if (order == null)
                        {
                            order = o;

                            if (org.OrganizationTypeId == 1)
                                o.Organization = org;
                            else if (org.OrganizationTypeId == 3)
                                o.Distributor = org;
                        }

                        order.OrderDetailings.Add(od);

                        return o;
                    });

                return order;
            }
        }

        public async Task<Statistics> GetOrdersStatisticsAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Statistics>(
                    sql: "dbo.getOrdersStatistics",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<OrderSummary>> GetSummary(OrderSummaryFilter orderSummaryFilter)
        {
            throw new System.NotImplementedException();
        }
    }
}
