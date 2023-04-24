using Dapper;
using Lima.Core.Tenant;
using Lima.Visits.Api.Domain;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Visits.Api.Repositories
{
    public class VisitsRepository : IVisitsRepository
    {
        private ITenantContext _tenantContext;

        public VisitsRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<IEnumerable<VisitInfo>> GetLastVisitsToDoctorsAsync(int offset, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<VisitInfo, Medrep, Organization, Doctor, VisitInfo>(
                    sql: "dbo.getLastVisitsToDoctors",
                    param: new { @offset, @userId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "VisitId,MedrepId,OrganizationId,DoctorId",
                    map: (v, m, o, d) =>
                    {
                        v.Medrep = m;
                        v.Organization = o;
                        v.Doctor = d;

                        return v;
                    });
            }
        }

        public async Task<IEnumerable<VisitInfo>> GetLastVisitsToPharmaciesAndDistributorsAsync(int offset, int userId )
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<VisitInfo, Medrep, Organization, Doctor, VisitInfo>(
                    sql: "dbo.getLastVisitsToDoctors",
                    param: new { @offset, @userId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "VisitId,MedrepId,OrganizationId,DoctorId",
                    map: (v, m, o, d) =>
                    {
                        v.Medrep = m;
                        v.Organization = o;
                        v.Doctor = d;

                        return v;
                    });
            }
        }

        public async Task<IEnumerable<Drug>> GetDrugsFromVisitsToDoctorsAsync(int visitId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Drug>(
                    sql: @"SELECT dd.Id, dd.FullName FROM VISITS v
	                        LEFT JOIN VISITS_DRUG vd ON vd.VisitId = v.Id
	                        LEFT JOIN DICT_DRUGS dd ON dd.Id = vd.DrugId
	                        WHERE v.Id = @visitId",
                    param: new { @visitId });
            }
        }

        public async Task<IEnumerable<VisitInfo>> GetAllVisitsToDoctorsAsync(VisitFilter filter, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<VisitInfo> visits = new List<VisitInfo>();

                await connection.QueryAsync<VisitInfo, Medrep, Organization, Doctor, Drug, VisitInfo>(
                    sql: "dbo.getAllVisitsToDoctors",
                    param: new { @userId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "VisitId,MedrepId,OrganizationId,DoctorId,Id",
                    map: (v, m, o, d, dr) =>
                    {
                        int index = visits.FindIndex(f => f.VisitId == v.VisitId);

                        if (index == -1)
                        {
                            visits.Add(v);
                            index = visits.Count - 1;
                        }

                        visits[index].Medrep = m;
                        visits[index].Organization = o;
                        visits[index].Doctor = d;

                        if (dr != null)
                            visits[index].TalkedAboutDrugs.Add(dr);

                        return v;
                    });

                
                if (filter.StatusId > 0)
                {
                    visits = visits.Where(w => w.VisitStatusId == filter.StatusId).ToList();
                }

                if (filter.Dates != null && filter.Dates.Length > 0)
                {
                    visits = visits.Where(w => w.DateCreate >= filter.Dates.First() && w.DateCreate <= filter.Dates.Last()).ToList();
                }

                if (!string.IsNullOrEmpty(filter.Phone))
                {
                    visits = visits.Where(w => w.Doctor?.DoctorPhone == filter.Phone).ToList();
                }

                if (filter.DoctorId > 0)
                {
                    visits = visits.Where(w => w.Doctor?.DoctorId == filter.DoctorId).ToList();
                }

                if (filter.MedrepId > 0)
                {
                    visits = visits.Where(w => w.Medrep?.MedrepId == filter.MedrepId).ToList();
                }

                if (filter.OrganizationId > 0)
                {
                    visits = visits.Where(w => w.Organization?.OrganizationId == filter.OrganizationId).ToList();
                }

                //visits = visits
                //    .Skip(filter.Offset)
                //    .Take(10)
                //    .ToList();

                return visits;
            }
        }

        public async Task<IEnumerable<VisitInfo>> GetAllVisitsToDistributorsAsync(VisitFilter filter, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var visits = await connection.QueryAsync<VisitInfo, Medrep, Organization, VisitInfo>(
                    sql: "dbo.getAllVisitsToDistributors",
                    param: new { @userId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "VisitId,MedrepId,OrganizationId",
                    map: (v, m, o) =>
                    {
                        v.Medrep = m;
                        v.Organization = o;

                        return v;
                    });

                if (filter.StatusId > 0)
                {
                    visits = visits.Where(w => w.VisitStatusId == filter.StatusId);
                }

                if (filter.Dates != null && filter.Dates.Length > 0)
                {
                    visits = visits.Where(w => w.DateCreate >= filter.Dates.First() && w.DateCreate <= filter.Dates.Last());
                }

                if (!string.IsNullOrEmpty(filter.Phone))
                {
                    visits = visits.Where(w => w.Doctor?.DoctorPhone == filter.Phone);
                }

                if (filter.MedrepId > 0)
                {
                    visits = visits.Where(w => w.Medrep?.MedrepId == filter.MedrepId);
                }

                if (filter.OrganizationId > 0)
                {
                    visits = visits.Where(w => w.Organization?.OrganizationId == filter.OrganizationId);
                }

                return visits;
            }
        }

        public async Task<IEnumerable<VisitInfo>> GetAllVisitsToPharmaciesAsync(VisitFilter filter, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<VisitInfo> visitsTemp = new List<VisitInfo>();

                await connection.QueryAsync<VisitInfo, Medrep, Organization, SelectedDrug, Drug, VisitInfo>(
                    sql: "dbo.getAllVisitsToPharmacies",
                    param: new { @userId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "VisitId,MedrepId,OrganizationId,DrugId,DrugId",
                    map: (v, m, o, d, td) =>
                    {
                        int index = visitsTemp.FindIndex(f => f.VisitId == v.VisitId);

                        if (index == -1)
                        {
                            visitsTemp.Add(v);

                            v.Medrep = m;

                            if (o.TypeId == 1)
                                v.Organization = o;

                            index = visitsTemp.Count - 1;
                        }

                        if (d != null)
                            visitsTemp[index].Drugs.Add(d);

                        if (td != null)
                            visitsTemp[index].TalkedAboutDrugs.Add(td);

                        return v;
                    });

                IEnumerable<VisitInfo> visits = visitsTemp;

                if (filter.StatusId > 0)
                {
                    visits = visits.Where(w => w.VisitStatusId == filter.StatusId);
                }

                if (filter.Dates != null && filter.Dates.Length > 0)
                {
                    visits = visits.Where(w => w.DateCreate >= filter.Dates.First() && w.DateCreate <= filter.Dates.Last());
                }

                if (!string.IsNullOrEmpty(filter.Phone))
                {
                    visits = visits.Where(w => w.Doctor?.DoctorPhone == filter.Phone);
                }

                if (filter.DoctorId > 0)
                {
                    visits = visits.Where(w => w.Doctor?.DoctorId == filter.DoctorId);
                }

                if (filter.MedrepId > 0)
                {
                    visits = visits.Where(w => w.Medrep?.MedrepId == filter.MedrepId);
                }

                if (filter.OrganizationId > 0)
                {
                    visits = visits.Where(w => w.Organization?.OrganizationId == filter.OrganizationId);
                }

                if (filter.Sales != null && filter.Sales.Length > 0)
                {
                    //visits = visits.Where(w => w.)
                    //conditions.Add($"sd.TotalSale BETWEEN {filter.Sales.First()} AND {filter.Sales.Last()}");
                }

                return visits;
                //return visits
                //    .Skip(filter.Offset)
                //    .Take(10);
            }
        }

        public async Task<IEnumerable<VisitInfo>> GetAllVisits(VisitFilter filter, int userId, int typeId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<VisitInfo> visitsTemp = new List<VisitInfo>();

                await connection.QueryAsync<VisitInfo, Medrep, Organization, Doctor, SelectedDrug, Drug, VisitInfo>(
                    sql: "dbo.getAllVisits",
                    param: new { @userId, @typeId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "VisitId,MedrepId,OrganizationId,DoctorId,DrugId,DrugId",
                    map: (v, m, o, doc, d, td) =>
                    {
                        int index = visitsTemp.FindIndex(f => f.VisitId == v.VisitId);

                        if (index == -1)
                        {
                            visitsTemp.Add(v);

                            v.Doctor = doc;
                            v.Medrep = m;
                            v.Organization = o;

                            index = visitsTemp.Count - 1;
                        }

                        if (d != null)
                            visitsTemp[index].Drugs.Add(d);

                        if (td != null)
                            visitsTemp[index].TalkedAboutDrugs.Add(td);

                        return v;
                    });

                IEnumerable<VisitInfo> visits = visitsTemp;

                if (filter.StatusId > 0)
                {
                    visits = visits.Where(w => w.VisitStatusId == filter.StatusId);
                }

                if (filter.Dates != null && filter.Dates.Length > 0)
                {
                    visits = visits.Where(w => w.DateCreate >= filter.Dates.First() && w.DateCreate <= filter.Dates.Last());
                }

                if (!string.IsNullOrEmpty(filter.Phone))
                {
                    visits = visits.Where(w => w.Doctor?.DoctorPhone == filter.Phone);
                }

                if (filter.DoctorId > 0)
                {
                    visits = visits.Where(w => w.Doctor?.DoctorId == filter.DoctorId);
                }

                if (!string.IsNullOrEmpty(filter.DoctorName?.Trim()))
                {
                    string doctorName = filter.DoctorName.ToLower().Trim();

                    visits = visits.Where(w => w.Doctor?.DoctorName?.ToLower()?.Contains(doctorName) == true).ToList();
                }

                if (filter.MedrepId > 0)
                {
                    visits = visits.Where(w => w.Medrep?.MedrepId == filter.MedrepId);
                }

                if (!string.IsNullOrEmpty(filter.MedrepName))
                {
                    string medrepName = filter.MedrepName.ToLower().Trim();

                    visits = visits.Where(w => w.Medrep.MedrepFullName.ToLower().Contains(medrepName)).ToList();
                }

                if (filter.OrganizationId > 0)
                {
                    visits = visits.Where(w => w.Organization?.OrganizationId == filter.OrganizationId);
                }

                if (!string.IsNullOrEmpty(filter.OrganizationName?.Trim()))
                {
                    string organizationName = filter.OrganizationName.ToLower().Trim();

                    visits.Where(w => w.Organization?.OrganizationName?.ToLower()?.Contains(organizationName) == true ||
                                      w.Distributor?.OrganizationName?.ToLower()?.Contains(organizationName) == true);
                }

                if (filter.Sales != null && filter.Sales.Length == 2)
                {
                    visits = visits.Where(w => w.TotalSum >= filter.Sales[0] && w.TotalSum <= filter.Sales[1]);
                }

                if (filter.Prepayments != null && filter.Prepayments.Length == 2)
                {
                    visits = visits.Where(w => w.PrepaymentSum >= filter.Prepayments[0] && w.PrepaymentSum <= filter.Prepayments[1]);
                }

                return visits;
                //return visits
                //    .Skip(filter.Offset)
                //    .Take(10);
            }
        }

        public async Task<IEnumerable<Balance>> GetDrugStoreBalanceAsync(int organizationId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Balance>(
                    sql: "dbo.getStockInDrugStore",
                    param: new { organizationId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Balance>> GetDistributorBalanceAsync(int organizationId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Balance>(
                    sql: "dbo.getStockFromDistributor",
                    param: new { organizationId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        private async Task<int> AddOrUpdateVisitAsync(Visit newVisit, int userId, SqlConnection externalConnection = null, SqlTransaction externalTransaction = null)
        {
            SqlConnection connection = externalConnection ?? new SqlConnection(_tenantContext.CurrentTenant.ConnectionString);

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            SqlTransaction transaction = externalTransaction ?? connection.BeginTransaction();

            try
            {
                int visitId = await connection.QueryFirstOrDefaultAsync<int>(
                        sql: "dbo.insertOrUpdateVisit",
                        commandType: CommandType.StoredProcedure,
                        param: new
                        {
                            @id = newVisit.VisitId,
                            @organizationId = newVisit.OrganizationId,
                            @organizationName = newVisit.OrganizationName,
                            @doctorId = newVisit.DoctorId,
                            @doctorName = newVisit.DoctorName,
                            @doctorPositionName = newVisit.DoctorPosition,
                            @doctorPhone = newVisit.DoctorPhone,
                            @comment = newVisit.Comment,
                            @userId = userId,
                            @prepayment = newVisit.Prepayment,
                            @typeId = newVisit.TypeId,
                            @statusId = newVisit.StatusId
                        },
                        transaction: transaction);

                if (newVisit.Drugs != null && newVisit.Drugs.Length > 0)
                {
                    DataTable visitDrugDataTable = new DataTable("VISIT_DRUG");
                    visitDrugDataTable.Columns.Add("DrugId");
                    visitDrugDataTable.Columns.Add("DrugsCount");
                    

                    newVisit.Drugs
                        .ToList()
                        .ForEach(f => visitDrugDataTable.Rows
                            .Add(f.DrugId == 0 ? (int?)null : f.DrugId,
                                 f.Package));

                    await connection.ExecuteAsync(
                        sql: "dbo.insertOrUpdateVisitDetailing",
                        commandType: CommandType.StoredProcedure,
                        param: new
                        {
                            @visitId = visitId,
                            @drugs = visitDrugDataTable.AsTableValuedParameter()
                        },
                        transaction: transaction);
                }

                if (externalTransaction == null)
                {
                    await transaction.CommitAsync();
                    await transaction.DisposeAsync();
                }
                   
                return visitId;
            }
            catch (System.Exception)
            {
                if (externalTransaction == null)
                    await transaction.RollbackAsync();

                throw;
            }  
            finally
            {
                if (externalConnection == null && connection.State != ConnectionState.Closed)
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<int> AddOrUpdateVisitToDoctorAsync(Visit newVisit, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int visitId = await AddOrUpdateVisitAsync(newVisit, userId, connection, transaction);

                        if (newVisit.TalkedAboutTheDrugsIds != null && newVisit.TalkedAboutTheDrugsIds.Length > 0)
                        {
                            await connection.QueryAsync(
                                sql: "DELETE FROM VISIT_TALKED_ABOUT_THE_DRUG WHERE VisitId = @visitId",
                                param: new { @visitId },
                                transaction: transaction);

                            for (int i = 0; i < newVisit.TalkedAboutTheDrugsIds.Length; i++)
                            {
                                await connection.QueryAsync(
                                    sql: "INSERT INTO VISIT_TALKED_ABOUT_THE_DRUG (VisitId, DrugId) VALUES (@visitId, @drugId)",
                                    param: new { @visitId, @drugId = newVisit.TalkedAboutTheDrugsIds[i] },
                                    transaction: transaction);
                            }
                        }

                        await transaction.CommitAsync();

                        return visitId;
                    }
                    catch (System.Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<int> AddOrUpdateVisitToDrugStore(Visit newVisit, int userId) 
            => await AddOrUpdateVisitAsync(newVisit, userId);

        public async Task<int> AddOrUpdateVisitToDistributorAsync(Visit newVisit, int userId)
            => await AddOrUpdateVisitAsync(newVisit, userId);

        public async Task<bool> AddDrugStoreBalanceAsync(Visit newVisit, int userId)
            => await AddOrUpdateVisitAsync(newVisit, userId) > 0;

        public async Task<bool> AddDistributorBalanceAsync(Visit newVisit, int userId)
           => await AddOrUpdateVisitAsync(newVisit, userId) > 0;

        public async Task<DoctorStatistics> GetStatisticsOnVisitsToDoctorAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<DoctorStatistics>(
                    sql: "dbo.getStatisticsOnVisitsToDoctors",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PharmacyStatistics> GetStatisticsOnVisitsToPharmacyAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<PharmacyStatistics>(
                    sql: "dbo.getStatisticsOnVisitsToPharmacy",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<DistributorStatistics> GetStatisticsOnVisitsToDistributorAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<DistributorStatistics>(
                    sql: "dbo.getStatisticsOnVisitsToDistributor",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task CompleateVisitAsync(int visitId, double latitude, double longitude)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.QueryAsync(
                            sql: "dbo.updateVisitStatus",
                            param: new { @visitId, @statusId = 3 },
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        await connection.QueryAsync(
                            sql: "UPDATE VISITS SET Latitude = @latitude, Longitude = @longitude WHERE Id = @visitId",
                            param: new { @visitId, @latitude, @longitude },
                            transaction: transaction);

                        await transaction.CommitAsync();
                    }
                    catch (System.Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<Visit> GetVisitByIdAsync(int visitId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                Visit visit = null;

                List<SelectedDrug> selectedDrugs = new List<SelectedDrug>();

                await connection.QueryAsync<Visit, SelectedDrug, Visit>(
                    sql: "dbo.getVisitById",
                    param: new { @visitId },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "VisitId,DrugId",
                    map: (v, d) => 
                    {
                        if (visit == null)
                            visit = v;

                        if (d != null && d.DrugId > 0)
                            selectedDrugs.Add(d);

                        return v;
                    });

                if (visit != null)
                    visit.Drugs = selectedDrugs.ToArray();

                return visit;
            }
        }

        public async Task<int> GetCountVisitsToDoctorsAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteScalarAsync<int>(
                    sql: "dbo.getCountVisitsToDoctors",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
