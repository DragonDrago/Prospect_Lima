using Lima.Statistics.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Statistics.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private IStatisticsRepository _statisticsRepository;
        private ClaimsPrincipal _claimsPrincipal;

        public StatisticsController(IStatisticsRepository statisticsRepository, ClaimsPrincipal claimsPrincipal)
        {
            _statisticsRepository = statisticsRepository;
            _claimsPrincipal = claimsPrincipal;
        }

        [HttpGet("common")]
        public async Task<IActionResult> Index()
        {
            if (_claimsPrincipal.Claims.Any(a => a.Type == "id") &&
                _claimsPrincipal.Claims.Any(a => a.Type == "cuid"))
            {
                var userId = Convert.ToInt32(_claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id").Value);
                var cuid = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "cuid").Value;

                return Ok(await _statisticsRepository.GetStatisticsAsync(cuid, userId));
            }

            throw new Exception("Ошибка получения данных");
        }

        [HttpGet("sales-per-month")]
        public async Task<IActionResult> SalesPerMonth() 
            => Ok(await _statisticsRepository.GetSalesPerMonthAsync()); 

        [HttpGet("best-sales-per-month")]
        public async Task<IActionResult> BestSalesPerMonth()
            => Ok(await _statisticsRepository.GetBestSalesPerMonthAsync());

        [HttpGet("worst-sales-per-month")]
        public async Task<IActionResult> WorstSalesPerMonth()
            => Ok(await _statisticsRepository.GetWorstSalesPerMonthAsync());
    }
}
