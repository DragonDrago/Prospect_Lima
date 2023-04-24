using AutoMapper;
using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.Repositories;
using Lima.Dictionaries.Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private IOrganizationsRepository _organizationsRepository;
        private IMapper _mapper;
        private ILogger _logger;

        public OrganizationsController(IOrganizationsRepository organizationsRepository, IMapper mapper, ILogger logger)
        {
            _organizationsRepository = organizationsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("health-care-facility/find/{query}")]
        public async Task<IActionResult> FindByHealthCareFacility(string query)
            => Ok(await _organizationsRepository.FindAsync(query, 2));


        [HttpGet("drug-stores/find/{query}")]
        public async Task<IActionResult> FindByDrugStores(string query)
            => Ok(await _organizationsRepository.FindAsync(query, 1));


        [HttpGet("distributors/find/{query}")]
        public async Task<IActionResult> FindByDistributors(string query)
            => Ok(await _organizationsRepository.FindAsync(query, 3));

        [HttpGet("find/{query}")]
        public async Task<IActionResult> Find(string query)
        {
            return Ok(await _organizationsRepository.FindAsync(query, -1));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            return Ok(await _organizationsRepository.GetByIdAsync(id));
        }

        [HttpGet("health-care-facility/types")]
        public async Task<IActionResult> GetHealthCareFacilityTypes() => Ok(await _organizationsRepository.GetHealthCareFacilityTypesAsync());

        [HttpPost("health-care-facility/add")]
        public async Task<IActionResult> AddHealthCareFacility([FromBody] HealthCareFacilityRequest healthCareFacilityRequest)
        {
            Organization organization = _mapper.Map<Organization>(healthCareFacilityRequest.Organization);
            
            //ЛПУ - TypeId == 2
            organization.TypeId = 2;

            return Ok(await _organizationsRepository.AddOrUpdateOrganizationAsync(organization, healthCareFacilityRequest.DoctorsIds));
        }

        [HttpPost("drug-stores/add")]
        public async Task<IActionResult> AddDrugStore([FromBody] OrganizationRequest organizationRequest)
        {
            Organization organization = _mapper.Map<Organization>(organizationRequest);

            //Аптека - TypeId == 1
            organization.TypeId = 1;

            return Ok(await _organizationsRepository.AddOrUpdateOrganizationAsync(organization));
        }

        [HttpPost("distributors/add")]
        public async Task<IActionResult> AddDistributor([FromBody] OrganizationRequest organizationRequest)
        {
            Organization organization = _mapper.Map<Organization>(organizationRequest);

            //Дистрибьютер - TypeId == 3
            organization.TypeId = 3;

            return Ok(await _organizationsRepository.AddOrUpdateOrganizationAsync(organization));
        }

        [HttpGet("distributors")]
        public async Task<IActionResult> Distributors([FromQuery] DistributorFilterRequest distributorFilterRequest)
        {
            DistributorFilter distributorFilter = _mapper.Map<DistributorFilter>(distributorFilterRequest);
            distributorFilter.Offset = distributorFilterRequest.Page * 10;
            distributorFilterRequest.Page = distributorFilterRequest.Page == 0 ? 1 : distributorFilterRequest.Page;


            return Ok(await _organizationsRepository.GetDistributorsAsync(distributorFilter));
        }

        [HttpGet("drug-stores")]
        public async Task<IActionResult> DrugStores([FromQuery] OrganizationFilterRequest drugStoresFilterRequest)
        {
            PharmacyFilter drugStoresFilter = _mapper.Map<PharmacyFilter>(drugStoresFilterRequest);
            drugStoresFilter.Offset = drugStoresFilterRequest.Page * 10;
            drugStoresFilter.Page = drugStoresFilterRequest.Page == 0 ? 1 : drugStoresFilterRequest.Page;

            return Ok(await _organizationsRepository.GetDrugStoresAsync(drugStoresFilter));
        }

        [HttpGet("health-care-facility")]
        public async Task<IActionResult> HealthCareFacility([FromQuery] HealthCareFacilityFilterRequest healthCareFacilityFilterRequest)
        {
            HealthCareFacilityFilter healthCareFacilityFilter = _mapper.Map<HealthCareFacilityFilter>(healthCareFacilityFilterRequest);
            healthCareFacilityFilter.Offset = healthCareFacilityFilterRequest.Page * 10;
            healthCareFacilityFilter.Page = healthCareFacilityFilterRequest.Page == 0 ? 1 : healthCareFacilityFilterRequest.Page;

            return Ok(await _organizationsRepository.GetHealthCareFacilityAsync(healthCareFacilityFilter));
        }

        [HttpPut("distributors/update/{id}")]
        public async Task<IActionResult> UpdateDistributor(int id, OrganizationRequest organizationRequest)
        {
            organizationRequest.Id = id;
            return await AddDistributor(organizationRequest);
        }

        [HttpPut("drug-stores/update/{id}")]
        public async Task<IActionResult> UpdateDrugStore(int id, OrganizationRequest organizationRequest)
        {
            organizationRequest.Id=id;
            return await AddDrugStore(organizationRequest);
        }

        [HttpPut("health-care-facility/update/{id}")]
        public async Task<IActionResult> UpdateHealthCareFacility(int id, HealthCareFacilityRequest organizationRequest)
        {
            organizationRequest.Organization.Id = id;
            return await AddHealthCareFacility(organizationRequest);
        }
    }
}
