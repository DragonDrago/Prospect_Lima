using Lima.Dictionaries.Api.Domain;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using System;
using Lima.Core.Tenant;

namespace Lima.Dictionaries.Api.Repositories
{
    public class DrugsRepository : IDrugsRepository
    {
        private string _connectionString;
        private ITenantContext _tenantContext;

        public DrugsRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<string> GetDrugPhotoName(int drugId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<string>(
                    sql: "SELECT PhotoName FROM DICT_DRUGS WHERE Id = @drugId",
                    param: new
                    {
                        @drugId
                    },
                    commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<int> AddOrUpdate(Drug drug)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteScalarAsync<int>(
                    sql: "dbo.insertOrUpdateDrug",
                    param: new 
                    { 
                        @id = drug.Id, 
                        @drugName = drug.DrugName.Trim(), 
                        @producerId = drug.ProducerId,
                        @basePrice = drug.BasePrice,
                        @countryId = drug.CountryId,
                        @photoName = drug.PhotoName,
                        @storePlaceId = drug.StorePlaceId,
                        @storePlaceName = drug.StorePlaceName?.Trim(),
                        @amount = drug.Amount,
                        @unitId = drug.UnitId
                    },
                    commandType: System.Data.CommandType.StoredProcedure); 
            }
        }

        public async Task<IEnumerable<Drug>> FindAsync(string text)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Drug>(
                    sql: @"dbo.getDrugsByName",
                    param: new { @text },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Drug>> All(DrugFilter drugFilter)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var drugs = await connection.QueryAsync<Drug>(
                    sql: @"dbo.getDrugs",
                    commandType: System.Data.CommandType.StoredProcedure);

                if (drugFilter != null)
                {
                    if (drugFilter.DrugId > 0)
                        drugs = drugs.Where(w => w.Id == drugFilter.DrugId);

                    if (!string.IsNullOrEmpty(drugFilter.DrugName?.Trim()))
                    {
                        string drugName = drugFilter.DrugName.ToLower().Trim();

                        drugs = drugs.Where(w => w.DrugName.ToLower().Contains(drugName));
                    }
                        
                    if (drugFilter.CountryId > 0)
                        drugs = drugs.Where(w => w.CountryId == drugFilter.CountryId);

                    if (drugFilter.StorePlaceId > 0)
                        drugs = drugs.Where((w) => w.StorePlaceId == drugFilter.StorePlaceId);

                    if (drugFilter.ExpireDateMonth > 0)
                        drugs = drugs.Where(w => w.ExpireDate <= DateTime.Now.AddMonths(drugFilter.ExpireDateMonth));
                
                    if (drugFilter.HaveAndOrder)
                        drugs = drugs.Where(w => w.DrugInOrderCount > 0);
                }

                return drugs.ToList();
            }
        }

        public async Task<int> Count()
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteScalarAsync<int>(
                    sql: "SELECT COUNT(*) FROM DICT_DRUGS WHERE IsDeleted = 0",
                    commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<Drug> FindById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Drug>(
                    sql: @"SELECT  dd.Id,
                            dd.FullName AS DrugName,
                            dd.ProducerId,
                            dp.Name AS ProducerName,
                            dd.StorePlaceId,
                            dd.ExpireDate,
                            dd.Amount,
                            dd.UnitId,
                            od.DrugInOrderCount

                        FROM DICT_DRUGS dd
                        LEFT JOIN LIMA_COMMON.dbo.DICT_PRODUCERS dp ON dp.Id = dd.ProducerId

                        LEFT JOIN
                        (
                            SELECT od.DrugId, COUNT(*) DrugInOrderCount
                            FROM ORDERS o
                            LEFT JOIN ORDERS_DETAILING od ON o.Id = od.DrugId
                            WHERE o.StatusId = 1
                            GROUP BY od.DrugId
                        ) od ON od.DrugId = dd.Id WHERE dd.Id = @id",
                    param: new { @id },
                    commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<bool> Remove(int id)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteScalarAsync<int>(
                    sql: "UPDATE DICT_DRUGS SET IsDeleted = 1 WHERE Id = @id",
                    param: new { @id },
                    commandType: System.Data.CommandType.Text) > 0;
            }
        }

        public async Task<IEnumerable<EntityBase>> GetUnitsAsync()
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<EntityBase>(
                    sql: "SELECT Id, Name FROM DICT_DRUG_UNITS",
                    commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<int> AddOrUpdateUnitAsync(EntityBase unit)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<int>(
                    sql: "dbo.insertOrUpdateDrugUnit",
                    param: new { @id = unit.Id, @name = unit.Name.Trim() },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<DrugSeries>> GetDrugSeriesAsync()
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<DrugSeries>(
                    sql: "dbo.getDrugsSeries",
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<int> AddOrUpdateDrugSeriesAsync(DrugSeries drugSeries)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<int>(
                    sql: "dbo.insertOrUpdateDrugSeries",
                    param: new 
                    { 
                        @id = drugSeries.Id,
                        @series = drugSeries.Series,
                        @expiredDate = drugSeries.ExpiredDate,
                        @drugId = drugSeries.DrugId,
                        @quantity = drugSeries.Quantity
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
