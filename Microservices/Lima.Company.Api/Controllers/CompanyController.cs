using Lima.Company.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Company.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private string _companyUid;

        private IUsersRepository _usersRepository;
        private ICompanyRepository _companyRepository;

        private ClaimsPrincipal _currentUser;

        public CompanyController(IUsersRepository usersRepository, ICompanyRepository companyRepository, ClaimsPrincipal claimsPrincipal)
        {
            _usersRepository = usersRepository;
            _companyRepository = companyRepository;
            _currentUser = claimsPrincipal;

            var cuidClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "cuid");

            _companyUid = cuidClaim.Value;
        }

        [HttpGet("users/find/{userName}")]
        public async Task<IActionResult> FindUser(string userName)
            => Ok(await _usersRepository.FindUserByNameAsync(userName, _companyUid));

        [HttpGet("markups")]
        public async Task<IActionResult> GetMarkups()
            => Ok(await _companyRepository.GetMarkupsAsync(_companyUid));

        [HttpGet("available-modules")]
        public async Task<IActionResult> GetAvailableModules()
            => Ok(await _companyRepository.GetAvailableModulesAsync(_companyUid));

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
            => Ok(await _companyRepository.GetRolesAsync(_companyUid));

        [HttpGet("license-info")]
        public async Task<IActionResult> GetLicenseInfo()
            => Ok(await _companyRepository.GetLicenseAsync(_companyUid));

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
            => Ok(await _usersRepository.GetProfile(Convert.ToInt32(_currentUser.Claims.FirstOrDefault(f => f.Type == "id").Value)));
    }
}
