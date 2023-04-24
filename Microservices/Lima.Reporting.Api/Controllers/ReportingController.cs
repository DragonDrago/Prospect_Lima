using AutoMapper;
using Lima.Reporting.Api.Domain;
using Lima.Reporting.Api.Repositories;
using Lima.Reporting.Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Reporting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ReportingController : ControllerBase
    {
        private IReportingRepository _reportingRepository;
        private IMapper _mapper;
        private int _userId;

        public ReportingController(IReportingRepository reportingRepository, IMapper mapper, ClaimsPrincipal claimsPrincipal)
        {
            _reportingRepository = reportingRepository;
            _mapper = mapper;

            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            _userId = Convert.ToInt32(userIdClaim.Value);
        }

        [HttpGet("common")]
        public async Task<IActionResult> CommonReport([FromQuery] ReportFilterRequest reportFilterRequest)
        {
            ReportFilter reportFilter = new ReportFilter();

            if (reportFilterRequest != null)
            {
                reportFilter = _mapper.Map<ReportFilter>(reportFilterRequest);
            }

            reportFilter.UserId = _userId;

            return Ok(await _reportingRepository.GetCommonReport(reportFilter));
        }
    }
}
