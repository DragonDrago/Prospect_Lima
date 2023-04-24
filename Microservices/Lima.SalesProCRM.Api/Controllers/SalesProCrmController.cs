using Lima.Leads.Api.Domain;
using Lima.Leads.Api.Repository;
using Lima.SalesProCRM.Api.Domains;
using Lima.SalesProCRM.Api.Dtos;
using Lima.SalesProCRM.Api.Repositories;
using Lima.SalesProCRM.Api.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lima.SalesProCRM.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalesProCrmController : ControllerBase
    {
        private readonly ISalesProCrmRepository salesProCrmRepository;
        private readonly ILeadsRepository leadsRepository;

        public SalesProCrmController(ISalesProCrmRepository salesProCrmRepository, ILeadsRepository leadsRepository)
        {
            this.salesProCrmRepository = salesProCrmRepository;
            this.leadsRepository = leadsRepository;
        }


        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove (int id)
        {
            var entity = await salesProCrmRepository.GetSalesById(id);
            if(entity== null)
            {
                return BadRequest(ModelState);
            }
            return Ok(salesProCrmRepository.DeleteSales(entity));
        }
        [HttpGet("sales")]
        public IActionResult AllSales([FromQuery] SalesProCrmDomainRequest salesProCrmDomainRequest)
        {
            return Ok(salesProCrmRepository.GetAllSales(salesProCrmDomainRequest));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] SalesProCrmDomain salesProCrmDomain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id != salesProCrmDomain.Id)
            {
                return BadRequest(ModelState);
            }
            var entity = new SalesProCrmDomain()
            {
                Id = id,
                DateTime = salesProCrmDomain.DateTime,
                Check = salesProCrmDomain.Check,
                LdiId = salesProCrmDomain.LdiId,
                LdiName = salesProCrmDomain.LdiName,
                TotalSum = salesProCrmDomain.TotalSum,
                ProductsId = salesProCrmDomain.ProductsId,
                LdiAttachedTo = salesProCrmDomain.LdiAttachedTo,
                LdiStatus = salesProCrmDomain.LdiStatus
            };
            return Ok( await salesProCrmRepository.UpdateSales(entity));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] SalesProCrmViewDomain salesProCrmViewDomain)
        {
            int[] productsId = new int[] { };
            decimal totalPriceFromView = decimal.MinValue;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            for(int i = 0; i<salesProCrmViewDomain.productViewDomains.Count; i++)
            {
                var product = salesProCrmViewDomain.productViewDomains[i];
                productsId[i] = product.ProductId;
                totalPriceFromView = totalPriceFromView + product.ProductPriceByNumber;
            }
            var salesEntity = new SalesProCrmDomain()
            {
                ProductsId = productsId.ToString(),
                TotalSum = totalPriceFromView,
                DateTime = salesProCrmViewDomain.DateTime,
                Check = salesProCrmViewDomain.Check,
                LdiId = salesProCrmViewDomain.leadsDomain.Id,
                LdiName = salesProCrmViewDomain.leadsDomain.FullName,
                LdiAttachedTo = salesProCrmViewDomain.leadsDomain.AttachedTo,
                LdiStatus = salesProCrmViewDomain.leadsDomain.Status
            };
            return Ok( await salesProCrmRepository.AddSales(salesEntity)); 
        }
    }
}
