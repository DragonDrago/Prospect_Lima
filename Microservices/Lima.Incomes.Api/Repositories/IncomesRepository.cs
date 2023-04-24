using Dapper;
using Lima.Core.Tenant;
using Lima.Incomes.Api.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Incomes.Api.Repositories
{
    public class IncomesRepository : IIncomesRepository
    {
        private ITenantContext _tenantContext;

        public IncomesRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        /// <summary>
        /// Создает новый приход или обновляет существующий.
        /// </summary>
        /// <param name="newIncome">Приход.</param>
        /// <returns></returns>
        public async Task<int> AddOrUpdate(SaveIncome newIncome)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int incomeId = newIncome.Id > 0 ? newIncome.Id :
                            await connection.QueryFirstOrDefaultAsync<int>(
                                sql: "INSERT INTO INCOMES (Name) VALUES (@incomeName); SELECT IDENT_CURRENT('INCOMES');",
                                param: new { @incomeName = newIncome.IncomeName },
                                transaction: transaction);

                        DataTable incomeDetailingDataTable = new DataTable("INCOME_DETAILING");
                        incomeDetailingDataTable.Columns.Add("IncomeId");
                        incomeDetailingDataTable.Columns.Add("IncomeDetailingId");
                        incomeDetailingDataTable.Columns.Add("DrugId");
                        incomeDetailingDataTable.Columns.Add("ProducerId");
                        incomeDetailingDataTable.Columns.Add("ExpireDate");
                        incomeDetailingDataTable.Columns.Add("DrugsCount");

                        foreach (var incomeDetailingItem in newIncome.IncomeDetailings)
                        {
                            incomeDetailingDataTable.Rows.Add(
                                incomeId,
                                incomeDetailingItem.IncomeDetailingId,
                                incomeDetailingItem.DrugId,
                                incomeDetailingItem.ProducerId,
                                incomeDetailingItem.ExpireDate,
                                incomeDetailingItem.DrugsCount);
                        }

                        await connection.QueryAsync(
                            sql: "dbo.insertOrUpdateIncomeDetailing",
                            param: new { @incomeDetailings = incomeDetailingDataTable.AsTableValuedParameter() },
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        await transaction.CommitAsync();

                        return incomeId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }   
            }
        }

        /// <summary>
        /// Получает статистику по товарам из приходов.
        /// </summary>
        /// <param name="incomeSummaryFilter">Фильтр.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IncomeSummary>> Summary(IncomeSummaryFilter incomeSummaryFilter)
        {
            using(SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                List<string> conditions = new List<string>();

                if (!string.IsNullOrEmpty(incomeSummaryFilter.DrugName))
                {
                    conditions.Add($"dd.FullName='{incomeSummaryFilter.DrugName}'");
                }

                if (!string.IsNullOrEmpty(incomeSummaryFilter.ProducingCountry))
                {
                    conditions.Add($"dc.Name='{incomeSummaryFilter.ProducingCountry}'");
                }

                if (!string.IsNullOrEmpty(incomeSummaryFilter.ProducerName))
                {
                    conditions.Add($"dp.Name='{incomeSummaryFilter.ProducerName}'");
                }

                if (incomeSummaryFilter.ExistsOrder != null)
                {
                    conditions.Add($"OrderCount {(incomeSummaryFilter.ExistsOrder == true ? "> 0" : "= 0")}");
                }

                string sqlConditions = string.Join(" AND ", conditions);

                await connection.OpenAsync();

                return await connection.QueryAsync<Income, IncomeSummary, Drug, IncomeSummary>(
                    sql: "dbo.getIncomeSummary",
                    param: new { criteria = sqlConditions },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id,IncomeDetailingId,DrugId",
                    map: (i, id, d) =>
                    {
                        id.Income = i;
                        id.Drug = d;

                        return id;
                    });
            }
        }

    }
}
