using Lima.Dictionaries.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private ICommonRepository _commonRepository;

        public CommonController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> Countries() => Ok(await _commonRepository.GetCountriesAsync());

        [HttpGet("regions/{countryId}")]
        public async Task<IActionResult> Regions(int countryid) => Ok(await _commonRepository.GetRegionsAsync(countryid));

        [HttpGet("areas/{regionId}")]
        public async Task<IActionResult> Areas(int regionId) => Ok(await _commonRepository.GetAreasAsync(regionId));

        [HttpGet("producers")]
        public async Task<IActionResult> Producers() => Ok(await _commonRepository.GetProducersAsync());
    }
}
