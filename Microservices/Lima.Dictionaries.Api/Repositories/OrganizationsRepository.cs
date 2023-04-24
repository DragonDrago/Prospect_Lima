using Dapper;
using Lima.Core.Tenant;
using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public class OrganizationsRepository : IOrganizationsRepository
    {
        private ITenantContext _tenantContext;
        private string _limaCommonConnectionString;

        public OrganizationsRepository()
        {
        }

        public OrganizationsRepository(ITenantContext tenantContext, string limaCommonConnectionString)
        {
            _tenantContext = tenantContext;
            _limaCommonConnectionString = limaCommonConnectionString;
        }

        public async Task<IEnumerable<EntityBase>> GetHealthCareFacilityTypesAsync()
        {
            using (SqlConnection connection = new SqlConnection(_limaCommonConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<EntityBase>(
                    sql: "SELECT Id, Name FROM DICT_HEALTH_CARE_FACILITY_TYPE");
            }
        }

        public async Task<int> AddOrUpdateOrganizationAsync(Organization organization, List<int> doctorsIds = null)
        {
            using (SqlConnection connection = new SqlConnection(_limaCommonConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int organizationId = connection.ExecuteScalar<int>(
                            sql: "dbo.insertOrUpdateOrganization",
                            param: new
                            {
                                @id = organization.Id,
                                @name = organization.Name.Trim(),
                                @address = organization.Address?.Trim(),
                                @inn = organization.Inn?.Trim(),
                                @healthCareFacilityTypeId = organization.HealthCareFacilityTypeId == 0 ? null : (int?)organization.HealthCareFacilityTypeId,
                                @typeId = organization.TypeId,
                                @regionId = organization.RegionId,
                                @latitude = organization.Latitude,
                                @longitude = organization.Longitude
                            },
                            commandType: System.Data.CommandType.StoredProcedure,
                            transaction: transaction);

                        if (doctorsIds != null && doctorsIds.Count > 0)
                        {
                            foreach (var doctorId in doctorsIds)
                            {
                                await connection.ExecuteAsync(
                                    sql: "INSERT INTO DOCTORS_ORGANIZATIONS_RELATIONS (DoctorId, OrganizationId) VALUES (@doctorId, @organizationId)",
                                    param: new { doctorId, organizationId },
                                    transaction: transaction);
                            }
                        }

                        await transaction.CommitAsync();

                        return organizationId;
                    }
                    catch (System.Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<Organization>> FindAsync(string query, int typeId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<Organization>(
                    sql: "dbo.getOrganizationsByText",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new { @text = query, @typeId });
            }
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_limaCommonConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Organization>(
                    sql: "SELECT Id, Name, Address FROM DICT_ORGANIZATIONS WHERE Id = @id",
                    param: new { @id });
            }
        }

        public async Task<Response> GetDistributorsAsync(DistributorFilter distributorFilter)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<OrganizationExtend>(
                    sql: "dbo.getOrganizations",
                    param: new { @organizationType = 3 },
                    commandType: System.Data.CommandType.StoredProcedure);

                if (distributorFilter != null)
                {
                    if (!string.IsNullOrEmpty(distributorFilter.OrganizationName?.Trim()))
                    {
                        string organizationName = distributorFilter.OrganizationName.Trim().ToLower();

                        result = result.Where(w => w.Name.ToLower().Contains(organizationName));
                    }

                    if (!string.IsNullOrEmpty(distributorFilter.Inn?.Trim()))
                    {
                        result = result.Where(w => w.Inn == distributorFilter.Inn.Trim());
                    }

                    if (distributorFilter.Sales != null && distributorFilter.Sales.Length > 0)
                    {
                        result = result.Where(w => w.SaleTotalSum >= distributorFilter.Sales.First() && w.SaleTotalSum <= distributorFilter.Sales.Last());
                    }

                    if (distributorFilter.Orders != null && distributorFilter.Orders.Length > 0)
                    {
                        result = result.Where(w => w.OrderCount >= distributorFilter.Orders.First() && w.OrderCount <= distributorFilter.Orders.Last());
                    }

                    if (distributorFilter.Prepayment != null && distributorFilter.Prepayment.Length > 0)
                    {
                        result = result.Where(w => w.PrepaymentSum >= distributorFilter.Prepayment.First() && w.PrepaymentSum <= distributorFilter.Prepayment.Last());
                    }
                }

                Page page = new Page(result.Count(), distributorFilter.Page, 10);

                result = result
                    .Skip(distributorFilter.Offset)
                    .Take(10)
                    .ToList();

                return new Response()
                {
                    Page = page,
                    Result = result
                };
            }
        }

        public async Task<Response> GetDrugStoresAsync(PharmacyFilter pharmacyFilter)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<OrganizationExtend>(
                    sql: "dbo.getOrganizations",
                    param: new { @organizationType = 1 },
                    commandType: System.Data.CommandType.StoredProcedure);


                if (pharmacyFilter != null)
                {
                    if (!string.IsNullOrEmpty(pharmacyFilter.OrganizationName?.Trim()))
                    {
                        string organizationName = pharmacyFilter.OrganizationName?.Trim().ToLower();
                        
                        result = result.Where(w => w.Name.ToLower().Contains(organizationName));
                    }

                    if (!string.IsNullOrEmpty(pharmacyFilter.Inn?.Trim()))
                    {
                        result = result.Where(w => w.Inn == pharmacyFilter.Inn.Trim());
                    }

                    if (pharmacyFilter.Sales != null && pharmacyFilter.Sales.Length > 0)
                    {
                        result = result.Where(w => w.SaleTotalSum >= pharmacyFilter.Sales.First() && w.SaleTotalSum <= pharmacyFilter.Sales.Last());
                    }

                    if (pharmacyFilter.Orders != null && pharmacyFilter.Orders.Length > 0)
                    {
                        result = result.Where(w => w.OrderCount >= pharmacyFilter.Orders.First() && w.OrderCount <= pharmacyFilter.Orders.Last());
                    }

                    if (pharmacyFilter.Prepayment != null && pharmacyFilter.Prepayment.Length > 0)
                    {
                        result = result.Where(w => w.PrepaymentSum >= pharmacyFilter.Prepayment.First() && w.PrepaymentSum <= pharmacyFilter.Prepayment.Last());
                    }
                }

                Page page = new Page(result.Count(), pharmacyFilter.Page, 10);

                result = result
                    .Skip(pharmacyFilter.Offset)
                    .Take(10)
                    .ToList();

                return new Response()
                {
                    Page = page,
                    Result = result
                };
            }
        }

        public async Task<Response> GetHealthCareFacilityAsync(HealthCareFacilityFilter healthCareFacilityFilter)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<HealthCareFacility> healthCareFacilities = new List<HealthCareFacility>();

                await connection.QueryAsync<HealthCareFacility, Doctor, HealthCareFacility>(
                    sql: "dbo.getHealthCareFacility",
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id,Id",
                    map: (o, d) => 
                    { 
                        int index = healthCareFacilities.FindIndex(f => f.Id == o.Id);

                        if (index == -1)
                        {
                            healthCareFacilities.Add(o);
                            index = healthCareFacilities.Count - 1;
                        }

                        if (d != null)
                            healthCareFacilities[index].Doctors.Add(d);

                        return o;
                    });

                if (healthCareFacilityFilter != null)
                {
                    if (!string.IsNullOrEmpty(healthCareFacilityFilter.OrganizationName?.Trim()))
                    {
                        string organizationName = healthCareFacilityFilter.OrganizationName.Trim().ToLower();

                        healthCareFacilities = healthCareFacilities
                            .Where(w => w.Name.ToLower().Contains(organizationName))
                            .ToList();
                    }

                    if (!string.IsNullOrEmpty(healthCareFacilityFilter.Inn?.Trim()))
                    {
                        healthCareFacilities = healthCareFacilities
                            .Where(w => w.Inn == healthCareFacilityFilter.Inn.Trim())
                            .ToList();
                    }
                }

                Page page = new Page(healthCareFacilities.Count, healthCareFacilityFilter.Page, 10);


                healthCareFacilities = healthCareFacilities
                    .Skip(healthCareFacilityFilter.Offset)
                    .Take(10)
                    .ToList();

                return new Response() 
                { 
                    Page = page,
                    Result = healthCareFacilities
                };
            }
        }
    }
}
