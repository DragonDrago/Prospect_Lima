using AutoMapper;
using Lima.Incomes.Api.Model;
using Lima.Incomes.Api.Repositories;
using Lima.Incomes.Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lima.Incomes.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private IMapper _mapper;
        private IIncomesRepository _incomesRepository;

        public IncomesController(IMapper mapper, IIncomesRepository incomesRepository)
        {
            _mapper = mapper;
            _incomesRepository = incomesRepository;
        }

        [HttpGet("summary")]
        [Authorize]
        public async Task<IActionResult> Summary([FromQuery] SummaryFilterRequest summaryFilterRequest)
        {
            IncomeSummaryFilter filter = _mapper.Map<IncomeSummaryFilter>(summaryFilterRequest);

            return Ok(await _incomesRepository.Summary(filter));
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] IncomeRequest incomeRequest)
        {
            SaveIncome income = _mapper.Map<SaveIncome>(incomeRequest);

            return Ok(await _incomesRepository.AddOrUpdate(income));
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] IncomeRequest incomeRequest)
        {
            SaveIncome income = _mapper.Map<SaveIncome>(incomeRequest);

            return Ok(await _incomesRepository.AddOrUpdate(income));
        }
    }
}
