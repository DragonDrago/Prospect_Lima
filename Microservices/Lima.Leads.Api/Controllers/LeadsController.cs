using Lima.Leads.Api.Domain;
using Lima.Leads.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lima.Leads.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadsRepository leadsRepository;

        public LeadsController(ILeadsRepository leadsRepository)
        {
            this.leadsRepository = leadsRepository;
        }


        [HttpGet("all")]
        public IActionResult All([FromQuery] LeadsDomain leadsDomain)
        {
            return Ok(leadsRepository.GetAllLeads(leadsDomain));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] LeadsDomain leadsDomain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await leadsRepository.AddLeads(leadsDomain));
        }

        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var entity = await leadsRepository.GetLeadsById(id);
            if (entity == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(leadsRepository.DeleteLeads(entity));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] LeadsDomain leadsDomain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != leadsDomain.Id)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await leadsRepository.UpdateLeads(leadsDomain);
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                return NotFound("No record found against this Id..");
            }

            return Ok(leadsDomain);
        }

    }
}
