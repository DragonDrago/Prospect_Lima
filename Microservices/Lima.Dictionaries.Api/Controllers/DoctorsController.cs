using AutoMapper;
using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.Repositories;
using Lima.Dictionaries.Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private IDoctorsRepository _doctorsRepository;
        private IMapper _mapper;

        public DoctorsController(IDoctorsRepository doctorsRepository, IMapper mapper)
        {
            _doctorsRepository = doctorsRepository;
            _mapper = mapper;
        }

        [HttpGet("find/{text}")]
        public async Task<IActionResult> Find(string text)
        {
            if (text == null || text.Length < 3)
                return new EmptyResult();

            return Ok(await _doctorsRepository.FindAsync(text));
        }

        /// <summary>
        /// Получает список врачей.
        /// </summary>
        /// <param name="doctorRequest"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> All([FromQuery] DoctorFilterRequest doctorRequest)
        {
            DoctorFilter filter = _mapper.Map<DoctorFilter>(doctorRequest);
            filter.Offset = doctorRequest.Page * 10;

            return Ok(await _doctorsRepository.GetAll(filter));
        }

        /// <summary>
        /// Создает нового врача в системе.
        /// </summary>
        /// <param name="doctorRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] DoctorRequest doctorRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Doctor doctor = _mapper.Map<Doctor>(doctorRequest);

            return Ok(await _doctorsRepository.AddOrUpdate(doctor));
        }

        /// <summary>
        /// Обновляет данные врача.
        /// </summary>
        /// <param name="doctorRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorRequest doctorRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));

                throw new Exception(string.Join(", ", errorMessages));
            }

            Doctor doctor = _mapper.Map<Doctor>(doctorRequest);
            doctor.Id = id;

            return Ok(await _doctorsRepository.AddOrUpdate(doctor));
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Worked!");
    }
}
