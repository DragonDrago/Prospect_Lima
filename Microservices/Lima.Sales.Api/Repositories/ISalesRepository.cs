using Lima.Sales.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Sales.Api.Repositories
{
    public interface ISalesRepository
    {
        Task<IEnumerable<Sale>> GetSalesAsync(SalesFilter salesFilter, int userId);
        Task<IEnumerable<SaleDetailing>> GetSaleDetailingAsync(string checkId);
        Task<Sale> GetSaleAsync(int saleId);
        Task<IEnumerable<Prepayment>> GetPrepaymentsAsync(PrepaymentFilter prepaymentFilter, int userId);
        Task<Statistics> GetSaleStatisticsAsync(int userId);
        Task<int> AddSaleAsync(NewSale newSale, int userId);
        Task<bool> ChangeStatusAsync(int saleId, byte status);
        Task AssignUserWhoConfirmedTheSale(int saleId, int userId);
        Task AssignUserWhoCanceledTheSale(int saleId, int userId);
    }
}
