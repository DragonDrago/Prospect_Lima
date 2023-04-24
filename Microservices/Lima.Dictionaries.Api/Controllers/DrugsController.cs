using AutoMapper;
using Lima.Dictionaries.Api.Repositories;
using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Lima.Dictionaries.Api.Utils;
using Microsoft.Extensions.Logging;
using Lima.Dictionaries.Api.Services;
using Microsoft.Net.Http.Headers;

namespace Lima.Dictionaries.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DrugsController : ControllerBase
    {
        private IMapper _mapper;
        private ILogger _logger;
        private IDrugsRepository _drugsRepository;
        private IStaticContentService _staticContentService;

        public DrugsController(IDrugsRepository drugsRepository, IStaticContentService staticContentService, IMapper mapper, ILogger logger)
        {
            _drugsRepository = drugsRepository;
            _staticContentService = staticContentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("find/{query}")]
        public async Task<IActionResult> Find(string query)
            => Ok(await _drugsRepository.FindAsync(query));

        /// <summary>
        /// Получает список товаров.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Policy = "DRUGS_VIEW")]
        public async Task<IActionResult> All([FromQuery] DrugFilterRequest drugFilterRequest)
        {
            DrugFilter drugFilter = drugFilterRequest != null ?
                _mapper.Map<DrugFilter>(drugFilterRequest) : null;

            return Ok(await _drugsRepository.All(drugFilter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id) => Ok(await _drugsRepository.FindById(id));

        [HttpGet("count")]
        public async Task<IActionResult> Count() => Ok(await _drugsRepository.Count());

        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove(int id) => Ok(await _drugsRepository.Remove(id));

        [HttpPost("add")]
        [Authorize(Policy = "DRUGS_ADD")]
        public async Task<IActionResult> Add([FromBody] DrugRequest drugRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Drug drug = _mapper.Map<Drug>(drugRequest);

            return Ok(await _drugsRepository.AddOrUpdate(drug));
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "DRUGS_EDIT")]
        public async Task<IActionResult> Update(int id, [FromBody] DrugRequest drugRequest)
        {
            drugRequest.Id = id;
            return await Add(drugRequest);
        }

        [HttpGet("units")]
        public async Task<IActionResult> Units() => Ok(await _drugsRepository.GetUnitsAsync());

        [HttpPost("units/add")]
        public async Task<IActionResult> AddUnit([FromBody] EntityRequest entityRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception(string.Join(", ", ModelState.SelectMany(s => s.Value.Errors.Select(er => er.ErrorMessage))));
            }

            var unit = _mapper.Map<EntityBase>(entityRequest);

            return Ok(await _drugsRepository.AddOrUpdateUnitAsync(unit));
        }

        [HttpPut("units/update")]
        public async Task<IActionResult> UpdateUnit([FromBody] EntityRequest entityRequest) => await AddUnit(entityRequest);

        [HttpGet("series")]
        [Authorize(Policy = "DRUGS_ADD")]
        public async Task<IActionResult> Series() => Ok(await _drugsRepository.GetDrugSeriesAsync());

        [HttpPost("series/add")]
        public async Task<IActionResult> AddSeries([FromBody] DrugSeriesRequest drugSeriesRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            DrugSeries drugSeries = _mapper.Map<DrugSeries>(drugSeriesRequest);

            return Ok(await _drugsRepository.AddOrUpdateDrugSeriesAsync(drugSeries));
        }

        [HttpPut("series/update/{id}")]
        public async Task<IActionResult> UpdateSeries(int id, [FromBody] DrugSeriesRequest drugSeriesRequest)
        {
            drugSeriesRequest.Id = id;
            return await AddSeries(drugSeriesRequest);
        }
    }
}
