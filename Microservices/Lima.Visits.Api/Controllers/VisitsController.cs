using AutoMapper;
using Lima.Visits.Api.Domain;
using Lima.Visits.Api.Dto;
using Lima.Visits.Api.Repositories;
using Lima.Visits.Api.Requests;
using Lima.Visits.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using RestEase;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Visits.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class VisitsController : ControllerBase
    {
        private IVisitsRepository _visitsRepository;
        private IOrderService _orderService;
        private IMapper _mapper;
        private ILogger _logger;

        private ClaimsPrincipal _claimsPrincipal;
        private int _userId;

        public VisitsController(
            IVisitsRepository visitsRepository,
            IMapper mapper,
            IOrderService orderService,
            ILogger logger,
            ClaimsPrincipal claimsPrincipal)
        {
            _visitsRepository = visitsRepository;
            _mapper = mapper;
            _claimsPrincipal = claimsPrincipal;
            _orderService = orderService;
            _logger = logger;

            var userIdClaim = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (userIdClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            _userId = Convert.ToInt32(userIdClaim.Value);
        }

        #region Doctors
        /// <summary>
        /// Получает последние визиты к докторам.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("doctors/last")]
        public async Task<IActionResult> LastVisitsToDoctors([FromQuery] int? page)
            => Ok(await _visitsRepository.GetLastVisitsToDoctorsAsync((int)(page == null ? 0 : page * 10), _userId));

        /// <summary>
        /// Получает все визиты к докторам.
        /// </summary>
        /// <param name="visitFilterRequest">Фильтр.</param>
        /// <returns></returns>
        [HttpGet("doctors")]
        public async Task<IActionResult> AllVisitsToDoctors([FromQuery] VisitFilterRequest visitFilterRequest)
        {
            VisitFilter visitFilter = _mapper.Map<VisitFilter>(visitFilterRequest);
            visitFilter.Offset = visitFilterRequest.Page * 10;
            visitFilter.UserId = _userId;

            return Ok(await _visitsRepository.GetAllVisits(visitFilter, _userId, 2));
        }

        /// <summary>
        /// Добавляет новый визит к врачу.
        /// </summary>
        /// <param name="newVisitRequest">Новый визит.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost("doctors/add")]
        public async Task<IActionResult> AddVisitToDoctor([FromBody] NewVisitRequest newVisitRequest)
        {
            var dsKeyClaim = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (dsKeyClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            if ((newVisitRequest.OrganizationId == 0 || newVisitRequest.OrganizationId == null) && string.IsNullOrEmpty(newVisitRequest.OrganizationName?.Trim()))
            {
                ModelState.AddModelError("OrgError", "Выберите организацию из списка или введите новую.");
            }

            if (newVisitRequest.DoctorId == 0 && string.IsNullOrEmpty(newVisitRequest.DoctorName?.Trim()))
            {
                ModelState.AddModelError("DocError", "Выберите врача из списка или введите нового.");
            }

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            Visit visit = _mapper.Map<Visit>(newVisitRequest);
            visit.StatusId = 1;
            visit.TypeId = 2;

            return Ok(await _visitsRepository.AddOrUpdateVisitToDoctorAsync(visit, Convert.ToInt32(dsKeyClaim.Value)));
        }

        /// <summary>
        /// Обновляет существущий визит.
        /// </summary>
        /// <param name="newVisitRequest">Существующий визит</param>
        /// <returns></returns>
        [HttpPut("doctors/update/{id}")]
        public async Task<IActionResult> UpdateVisitToDoctor(int id, [FromBody] NewVisitRequest newVisitRequest)
        {
            newVisitRequest.VisitId = id;

            return await AddVisitToDoctor(newVisitRequest);
        }

        /// <summary>
        /// Возращает статистику посещений к врачам.
        /// </summary>
        /// <returns></returns>
        [HttpGet("doctors/statistics")]
        public async Task<IActionResult> VisitToDoctorStatistics()
            => Ok(await _visitsRepository.GetStatisticsOnVisitsToDoctorAsync(_userId));

        /// <summary>
        /// Получает товары.
        /// </summary>
        /// <param name="visitId"></param>
        /// <returns></returns>
        [HttpGet("{visitId}/doctors/drugs")]
        public async Task<IActionResult> DrugsFromVisitsToDoctors(int visitId)
             => Ok(await _visitsRepository.GetDrugsFromVisitsToDoctorsAsync(visitId));

        /// <summary>
        /// Возвращает количество визитов к докторам.
        /// </summary>
        /// <returns></returns>

        [HttpGet("doctors/count")]
        public async Task<IActionResult> GetCountVisitToDoctors()
            => Ok(new { Count = await _visitsRepository.GetCountVisitsToDoctorsAsync(_userId) });
        #endregion

        #region Distributors
        /// <summary>
        /// Добавляет новый визит к дистрибьютеру.
        /// </summary>
        /// <param name="pharmacyDistributorVisitRequest"></param>
        /// <returns></returns>
        [HttpPost("distributors/add")]
        public async Task<IActionResult> AddVisitToDistributor([FromBody] NewVisitRequest newVisitRequest)
        {
            var dsKeyClaim = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (dsKeyClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            //if (newVisitRequest.Drugs == null || newVisitRequest.Drugs.Length == 0)
            //{
            //    ModelState.AddModelError("DrugError", "Выберите товары.");
            //}

            if ((newVisitRequest.OrganizationId == 0 || newVisitRequest.OrganizationId == null) && string.IsNullOrEmpty(newVisitRequest.OrganizationName?.Trim()))
            {
                ModelState.AddModelError("OrgError", "Выберите организацию из списка или введите новую.");
            }

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            Visit visit = _mapper.Map<Visit>(newVisitRequest);
            visit.TypeId = 3;
            visit.StatusId = 1;

            int visitId = await _visitsRepository.AddOrUpdateVisitToDistributorAsync(visit, Convert.ToInt32(dsKeyClaim.Value));

            //if (visit.VisitId == 0)
            //{
            //    NewOrderDto newOrderDto = new NewOrderDto();
            //    newOrderDto.VisitId = visitId;
            //    newOrderDto.Name = $"Визит к дистрибьютеру. Бронь от {DateTime.Now}";
            //    newOrderDto.PrepaymentPercent = visit.Prepayment;
            //    newOrderDto.OrdersDetailing = visit.Drugs
            //        .Select(s => new NewOrderDetailingDto()
            //        {
            //            Amount = s.Package,
            //            DrugId = s.DrugId
            //        })
            //        .ToList();

            //    await _orderService.CreateAsync(newOrderDto, Request.Headers[HeaderNames.Authorization]);
            //}

            return Ok(visitId);
        }

        /// <summary>
        /// Обновляет существующий визит к дистрибьютеру.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newVisitRequest"></param>
        /// <returns></returns>
        [HttpPut("distributors/update/{id}")]
        public async Task<IActionResult> UpdateVisitToDistributor(int id, [FromBody] NewVisitRequest newVisitRequest)
        {
            newVisitRequest.VisitId = id;

            return await AddVisitToDistributor(newVisitRequest);
        }

        /// <summary>
        /// Возвращает список визитов к дистрибьютерам.
        /// </summary>
        /// <param name="visitFilterRequest"></param>
        /// <returns></returns>
        [HttpGet("distributors")]
        public async Task<IActionResult> AllVisitsToDistributors([FromQuery] VisitFilterRequest visitFilterRequest)
        {
            VisitFilter visitFilter = _mapper.Map<VisitFilter>(visitFilterRequest);
            visitFilter.Offset = visitFilterRequest.Page * 10;
            visitFilter.UserId = _userId;

            return Ok(await _visitsRepository.GetAllVisits(visitFilter, _userId, 3));
        }

        /// <summary>
        /// Фиксирует текущие остатки товаров у дистрибьютера.
        /// </summary>
        /// <param name="newVisitRequest"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost("distributors/balance/add")]
        public async Task<IActionResult> SaveDistributorBalance([FromBody] NewVisitRequest newVisitRequest)
        {
            var dsKeyClaim = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (newVisitRequest.Drugs == null || newVisitRequest.Drugs.Length == 0)
            {
                ModelState.AddModelError("DrugError", "Коллекция с остатками пустая.");
            }

            if (dsKeyClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            if ((newVisitRequest.OrganizationId == 0 || newVisitRequest.OrganizationId == null) && string.IsNullOrEmpty(newVisitRequest.OrganizationName?.Trim()))
            {
                ModelState.AddModelError("OrgError", "Выберите организацию из списка или введите новую.");
            }

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }


            if (newVisitRequest.OrganizationId != 0 || newVisitRequest.OrganizationId != null)
            {
                //получаем текущие остатки
                var currentBalance = await _visitsRepository.GetDistributorBalanceAsync((int)newVisitRequest.OrganizationId);

                //получаем разницу
                newVisitRequest.Drugs = currentBalance
                    .Join(newVisitRequest.Drugs, o => o.DrugId, i => i.DrugId, (o, i) => new DrugRequest()
                    {
                        DrugId = i.DrugId,
                        //IncomeDetailingId = i.IncomeDetailingId,
                        Package = o.DrugBalance - i.Package
                    })
                    .ToArray();
            }

            Visit visit = _mapper.Map<Visit>(newVisitRequest);

            //снятие остатков у дистрибьютера
            visit.TypeId = 5;

            //визит проведён успешно
            visit.StatusId = 3;

            //сохранение и возврат результата
            return Ok(await _visitsRepository.AddDistributorBalanceAsync(visit, Convert.ToInt32(dsKeyClaim.Value)));
        }

        /// <summary>
        /// Получает остатки дистрибьютера
        /// </summary>
        /// <param name="organizationId">Id дистрибьютера</param>
        /// <returns></returns>
        [HttpGet("distributors/{organizationId}/balance")]
        public async Task<IActionResult> DistributorBalance(int organizationId)
        {
            return Ok(await _visitsRepository.GetDistributorBalanceAsync(organizationId));
        }

        [HttpGet("distributors/statistics")]
        public async Task<IActionResult> StatisticsOnVisitsToDistributors()
            => Ok(await _visitsRepository.GetStatisticsOnVisitsToDistributorAsync(_userId));

        #endregion

        #region Pharmacies
        /// <summary>
        /// Добавляет новый визит в аптеку.
        /// </summary>
        /// <param name="pharmacyDistributorVisitRequest"></param>
        /// <returns></returns>
        [HttpPost("pharmacies/add")]
        public async Task<IActionResult> AddVisitToDrugStore([FromBody] NewVisitRequest newVisitRequest)
        {
            var dsKeyClaim = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (dsKeyClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            //if (newVisitRequest.Drugs == null || newVisitRequest.Drugs.Length == 0)
            //{
            //    ModelState.AddModelError("DrugError", "Выберите товары.");
            //}

            if ((newVisitRequest.OrganizationId == 0 || newVisitRequest.OrganizationId == null) && string.IsNullOrEmpty(newVisitRequest.OrganizationName?.Trim()))
            {
                ModelState.AddModelError("OrgError", "Выберите организацию из списка или введите новую.");
            }

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            Visit visit = _mapper.Map<Visit>(newVisitRequest);
            visit.TypeId = 1;
            visit.StatusId = 1;

            int visitId = await _visitsRepository.AddOrUpdateVisitToDrugStore(visit, Convert.ToInt32(dsKeyClaim.Value));

            //if (visit.VisitId == 0)
            //{
            //    NewOrderDto newOrderDto = new NewOrderDto();
            //    newOrderDto.VisitId = visitId;
            //    newOrderDto.Name = $"Визит в аптеку. Бронь от {DateTime.Now}";
            //    newOrderDto.PrepaymentPercent = visit.Prepayment;
            //    newOrderDto.OrdersDetailing = visit.Drugs
            //        .Select(s => new NewOrderDetailingDto()
            //        {
            //            Amount = s.Package,
            //            DrugId = s.DrugId
            //        })
            //        .ToList();

            //    await _orderService.CreateAsync(newOrderDto, Request.Headers[HeaderNames.Authorization]);
            //}


            return Ok(visitId);
        }

        /// <summary>
        /// Обновляет существующий визит в аптеку.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newVisitRequest"></param>
        /// <returns></returns>
        [HttpPut("pharmacies/update/{id}")]
        public async Task<IActionResult> UpdateVisitToPharmacy(int id, [FromBody] NewVisitRequest newVisitRequest)
        {
            newVisitRequest.VisitId = id;

            return await AddVisitToDrugStore(newVisitRequest);
        }

        /// <summary>
        /// Получает последние визиты к аптекам и дистрибьютерам.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <returns></returns>
        [HttpGet("pharmacies-and-distributors/last")]
        public async Task<IActionResult> LastVisitsToPharmaciesAndDistributors(int? page)
            => Ok(await _visitsRepository.GetLastVisitsToPharmaciesAndDistributorsAsync((int)(page == null ? 0 : page * 10), _userId));

        /// <summary>
        /// Получает все визиты к аптекам и дистрибютерам.
        /// </summary>
        /// <param name="visitFilterRequest">Фильтр.</param>
        /// <returns></returns>
        [HttpGet("pharmacies")]
        public async Task<IActionResult> AllVisitsToPharmaciesAndDistributors([FromQuery] VisitFilterRequest visitFilterRequest)
        {
            VisitFilter visitFilter = _mapper.Map<VisitFilter>(visitFilterRequest);
            visitFilter.Offset = visitFilterRequest.Page * 10;

            return Ok(await _visitsRepository.GetAllVisits(visitFilter, _userId, 1));
        }

        /// <summary>
        /// Получает остатки аптеки.
        /// </summary>
        /// <param name="organizationId">Id аптеки</param>
        /// <returns></returns>
        [HttpGet("pharmacies/{organizationId}/balance")]
        public async Task<IActionResult> DrugStoreBalance(int organizationId)
        {
            return Ok(await _visitsRepository.GetDrugStoreBalanceAsync(organizationId));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVisitRequest"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost("pharmacies/balance/add")]
        public async Task<IActionResult> SaveDrugStoreBalance([FromBody] NewVisitRequest newVisitRequest)
        {
            var dsKeyClaim = _claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (newVisitRequest.Drugs == null || newVisitRequest.Drugs.Length == 0)
            {
                ModelState.AddModelError("DrugError", "Коллекция с остатками пустая.");
            }

            if (dsKeyClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            if (newVisitRequest.OrganizationId == 0 || newVisitRequest.OrganizationId == null)
            {
                ModelState.AddModelError("OrgError", "Выберите организацию из списка.");
            }

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            if (newVisitRequest.OrganizationId != 0 || newVisitRequest.OrganizationId != null)
            {
                //получаем текущие остатки
                var currentBalance = await _visitsRepository.GetDrugStoreBalanceAsync((int)newVisitRequest.OrganizationId);

                //получаем разницу
                newVisitRequest.Drugs = currentBalance
                    .Join(newVisitRequest.Drugs, o => o.DrugId, i => i.DrugId, (o, i) => new DrugRequest()
                    {
                        DrugId = i.DrugId,
                    //IncomeDetailingId = i.IncomeDetailingId,
                    Package = o.DrugBalance - i.Package
                    })
                    .ToArray();
            }

            Visit visit = _mapper.Map<Visit>(newVisitRequest);

            //снятие остатков
            visit.TypeId = 4;

            //визит проведён успешно
            visit.StatusId = 3;

            //сохранение и возврат результата
            return Ok(await _visitsRepository.AddDrugStoreBalanceAsync(visit, Convert.ToInt32(dsKeyClaim.Value)));
        }

        /// <summary>
        /// Получает статистику визитов по аптекам.
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("pharmacies/statistics")]
        public async Task<IActionResult> StatisticsOnVisitsToPharmacy()
            => Ok(await _visitsRepository.GetStatisticsOnVisitsToPharmacyAsync(_userId));
        #endregion

        #region Common
        /// <summary>
        /// Завершает визит.
        /// </summary>
        /// <param name="compleateVisitRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="System.Exception"></exception>
        [HttpPost("compleate")]
        public async Task<IActionResult> CompleateVisit([FromBody] CompleateVisitRequest compleateVisitRequest)
        {
            if (compleateVisitRequest == null)
                throw new Exception("Нет данных.");

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            int? orderId = null;

            //получаем визит по Id для дальнейшей проверки
            var visit = await _visitsRepository.GetVisitByIdAsync(compleateVisitRequest.VisitId);

            //в случае, если визит уже был завершен или просрочен - выбросить исключение
            if (visit.StatusId == 3)
                throw new Exception("Визит уже завершен. Повторное выполнение операции запрещено.");

            if (visit.StatusId == 2)
                throw new Exception("Визит просрочен.");

            //завершение визита
            await _visitsRepository.CompleateVisitAsync(compleateVisitRequest.VisitId, compleateVisitRequest.Latitude, compleateVisitRequest.Longitude);

            if (visit == null)
                throw new Exception("Визит с таким Id не найден.");

            //если у визита имеется детализация с товарами, то создаём бронь
            if (visit.Drugs.Length > 0)
            {
                NewOrderDto newOrderDto = new NewOrderDto();
                newOrderDto.VisitId = visit.VisitId;
                newOrderDto.StatusId = 1;
                newOrderDto.Name = $"Бронь от {DateTime.Now}. Id визита: {visit.VisitId}";
                newOrderDto.PrepaymentPercent = visit.Prepayment;
                newOrderDto.OrdersDetailing = visit.Drugs
                    .Select(s => new NewOrderDetailingDto()
                    {
                        Amount = s.Package,
                        DrugId = s.DrugId
                    })
                    .ToList();

                //вызов сервиса по работе с бронями
                orderId = await _orderService.CreateAsync(newOrderDto, Request.Headers[HeaderNames.Authorization]);
            }

            return Ok(new
            {
                VisitId = compleateVisitRequest.VisitId,
                StatusId = visit.StatusId,
                Compleated = true,
                OrderId = orderId
            });
        }

        #endregion
    }
}
