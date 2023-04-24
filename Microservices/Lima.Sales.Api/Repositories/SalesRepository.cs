using Dapper;
using Lima.Core.Tenant;
using Lima.Sales.Api.Domain;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Sales.Api.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private ITenantContext _tenantContext;

        public SalesRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<int> AddSaleAsync(NewSale newSale, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int saleId = connection.ExecuteScalar<int>(
                            sql: "dbo.insertSale",
                            param: new
                            {
                                @orderId = newSale.OrderId,
                                @userId,
                                @name = newSale.Name,
                                @prepaymentPercent = newSale.PrepaymentPercent
                            },
                            transaction: transaction,
                            commandType: System.Data.CommandType.StoredProcedure);

                        DataTable dataTable = new DataTable("SALE_DETAILING");
                        dataTable.Columns.Add("SalePrice", typeof(decimal));
                        dataTable.Columns.Add("Amount", typeof(int));
                        dataTable.Columns.Add("DrugId", typeof(int));

                        newSale.SalesDetailing.ForEach(f => dataTable.Rows.Add(f.SalePrice, f.Amount, f.DrugId));

                        await connection.QueryAsync(
                            sql: "dbo.insertSalesDetailing",
                            param: new { @saleId, @salesDetailing = dataTable.AsTableValuedParameter() },
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        await transaction.CommitAsync();

                        return saleId;
                    }
                    catch (System.Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task AssignUserWhoCanceledTheSale(int saleId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                await connection.QueryAsync(
                    sql: "UPDATE SALES SET AcceptUserId = @userId WHERE Id = @saleId",
                    param: new { @saleId, @userId });
            }
        }

        public async Task AssignUserWhoConfirmedTheSale(int saleId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                await connection.QueryAsync(
                    sql: "UPDATE SALES SET CancelUserId = @userId WHERE Id = @saleId",
                    param: new { @saleId, @userId });
            }
        }

        public async Task<bool> ChangeStatusAsync(int saleId, byte status)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    sql: "UPDATE SALES SET StatusId = @statusId WHERE Id = @saleId",
                    param: new { @saleId, @statusId = @status}) > 0;
            }
        }

        public async Task<IEnumerable<Prepayment>> GetPrepaymentsAsync(PrepaymentFilter filter, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var prepayments = await connection.QueryAsync<Prepayment, Organization, Prepayment>(
                    sql: "dbo.getSalesPrepayments",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new { @userId },
                    splitOn: "SaleId,OrganizationId",
                    map: (s, o) =>
                    {
                        s.Organization = o;

                        return s;
                    });

                if (filter.SaleId > 0)
                    prepayments = prepayments.Where(w => w.SaleId == filter.SaleId);
                
                if (filter.OrganizationId > 0)
                    prepayments = prepayments.Where(w => w.Organization.OrganizationId == filter.OrganizationId);

                //if (filter.SaleSum != null && filter.SaleSum.Length > 0)
                   // prepayments.Where(w => w == filter.OrganizationId);

                if (filter.DrugCount != null && filter.DrugCount.Length > 0)
                    prepayments = prepayments.Where(w => w.DrugsCount >= filter.DrugCount.First() && w.DrugsCount <= filter.DrugCount.Last());

                if (filter.Prepayments != null && filter.Prepayments.Length > 0)
                    prepayments = prepayments.Where(w => w.PrepaymentSum >= filter.Prepayments.First() && w.PrepaymentSum <= filter.Prepayments.Last());


                return prepayments.ToList();
            }
        }

        public async Task<Sale> GetSaleAsync(int saleId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var sale = (await connection.QueryAsync<Sale, User, Organization, Sale>(
                    sql: "dbo.getSale",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new { @saleId },
                    splitOn: "SaleId,UserId,OrganizationId",
                    map: (s, u, o) =>
                    {
                        s.User = u;
                        s.Organization = o;

                        return s;
                    })).FirstOrDefault();

                return sale;
            }
        }

        public async Task<IEnumerable<SaleDetailing>> GetSaleDetailingAsync(string checkId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<SaleDetailing>(
                    sql: "dbo.getSaleDetailing",
                    param: new { @checkId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(SalesFilter filter, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var sales = await connection.QueryAsync<Sale, User, Organization, Sale>(
                    sql: "dbo.getSales",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new { @userId },
                    splitOn: "SaleId,UserId,OrganizationId",
                    map: (s, u, o) => 
                    { 
                        s.User = u;
                        s.Organization = o;

                        return s;
                    });

                if (filter.SaleId > 0)
                {
                    sales = sales.Where(s => s.Id == filter.SaleId);
                }

                if (filter.Dates != null && filter.Dates.Length > 0)
                {
                    sales = sales.Where(s => s.CreateDate >= filter.Dates.First() && s.CreateDate <= filter.Dates.Last());
                }

                if (filter.UserId > 0)
                {
                    sales = sales.Where(w => w.User.UserId == filter.UserId);
                }

                if (filter.OrganizationId > 0)
                {
                    sales = sales.Where(w => w.Organization.OrganizationId == filter.OrganizationId);
                }

                if (filter.SaleSum != null && filter.SaleSum.Length > 0)
                {
                    sales = sales.Where(w => w.SaleTotal >= filter.SaleSum.First() && w.SaleTotal <= filter.SaleSum.Last());
                }

                return sales.ToList();
            }
        }

        public async Task<Statistics> GetSaleStatisticsAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Statistics>(
                    sql: "dbo.getSaleStatistics",
                    param: new { @userId },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
