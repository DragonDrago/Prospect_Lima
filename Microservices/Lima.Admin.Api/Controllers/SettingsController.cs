using AutoMapper;
using Lima.Admin.Api.Domain;
using Lima.Admin.Api.Repositories;
using Lima.Admin.Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", Policy = "SETTINGS_VIEW")]
    public class SettingsController : ControllerBase
    {
        private string _companyUid;

        private IMapper _mapper;
        
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private IFilterRepository _filterRepository;

        public SettingsController(
            IMapper mapper,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IFilterRepository filterRepository,
            ClaimsPrincipal claimsPrincipal)
        {
            _mapper = mapper;

            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _filterRepository = filterRepository;

            var cuidClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "cuid");

            _companyUid = cuidClaim.Value;
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetCompany() 
            => Ok(await _companyRepository.GetCompanyAsync(_companyUid));

        [HttpPost("company")]
        public async Task<IActionResult> SaveCompany([FromBody] CompanyRequest companyRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Company company = _mapper.Map<Company>(companyRequest);

            return Ok(await _companyRepository.SaveCompanyAsync(company, _companyUid));
        }

        [HttpGet("branches")]
        public async Task<IActionResult> GetBranches()
            => Ok(await _companyRepository.GetBranchesAsync(_companyUid));

        [HttpPost("branches/add")]
        public async Task<IActionResult> AddBranch([FromBody] BranchRequest branchRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Branch branch = _mapper.Map<Branch>(branchRequest);

            return Ok(await _companyRepository.AddOrUpdateBranchAsync(branch, _companyUid));
        }

        [HttpPut("branches/update")]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchRequest branchRequest)
            => await AddBranch(branchRequest);
        
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
            => Ok(await _companyRepository.GetDepartmentAsync(_companyUid));

        [HttpPost("departments/add")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentRequest departmentRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Department department = _mapper.Map<Department>(departmentRequest);

            return Ok(await _companyRepository.AddOrUpdateDepartmentAsync(department, _companyUid));
        }

        [HttpPut("departments/update")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentRequest departmentRequest)
            => await AddDepartment(departmentRequest);

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
            => Ok(await _userRepository.GetUsersAsync(_companyUid));

        [HttpPost("users/add")]
        public async Task<IActionResult> AddUser([FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            User user = _mapper.Map<User>(userRequest);

            if (userRequest.Id > 0)
            {
                Domain.User foundUser = await _userRepository.FindUserAsync(userRequest.Id, _companyUid);

                if (!BCrypt.Net.BCrypt.Verify(userRequest.Password, foundUser.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
                }
            }

            return Ok(await _userRepository.AddOrUpdateUserAsync(user, _companyUid));
        }

        [HttpPut("users/update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest)
            => await AddUser(userRequest);

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
            => Ok(await _filterRepository.GetFiltersAsync(_companyUid));

        [HttpPost("filters/{filterId}/{enabled}")]
        public async Task<IActionResult> UpdateFilter(int filterId, bool enabled)
            => Ok(await _filterRepository.EnableFilterAsync(filterId, enabled, _companyUid));

        [HttpGet("markups")]
        public async Task<IActionResult> GetMarkups()
            => Ok(await _companyRepository.GetMarkupsAsync(_companyUid));

        [HttpPost("markups/add")]
        public async Task<IActionResult> AddMarkup([FromBody] MarkupRequest markupRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Markup markup = _mapper.Map<Markup>(markupRequest);

            return Ok(await _companyRepository.AddOrUpdateMarkupAsync(markup, _companyUid));
        }

        [HttpPut("markups/update")]
        public async Task<IActionResult> UpdateMarkup([FromBody] MarkupRequest markupRequest)
            => await AddMarkup(markupRequest);

        [HttpGet("roles")]
        public async Task<IActionResult> Roles() => Ok(await _companyRepository.GetRolesAsync(_companyUid));

        [HttpGet("storages")]
        public async Task<IActionResult> GetStorages()
            => Ok(await _companyRepository.GetStoragesAsync(_companyUid));

        [HttpPost("storages/add")]
        public async Task<IActionResult> AddStorage([FromBody] EntityRequest entityRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Entity entity = _mapper.Map<Entity>(entityRequest);

            return Ok(await _companyRepository.AddOrUpdateStorage(entity, _companyUid));
        }

        [HttpPut("storages/update/{id}")]
        public async Task<IActionResult> UpdateStorage(int id, [FromBody] EntityRequest entityRequest)
        {
            entityRequest.Id = id;
            return await AddStorage(entityRequest);
        }


    }
}
