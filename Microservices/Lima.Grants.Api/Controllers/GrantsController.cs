using Lima.Grants.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Grants.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GrantsController : ControllerBase
    {
        private IGrantsRepository _grantsRepository;

        public GrantsController(IGrantsRepository grantsRepository)
        {
            _grantsRepository = grantsRepository;
        }

        [HttpGet("get/{userId}/{cuid}")]
        public async Task<IActionResult> Grants(int userId, string cuid)
            => Ok(await _grantsRepository.GetUserGrantsAsync(userId, cuid)); //Ok(await _grantsRepository.GetUserGrantsAsync(userId, cuid));
    }
}
