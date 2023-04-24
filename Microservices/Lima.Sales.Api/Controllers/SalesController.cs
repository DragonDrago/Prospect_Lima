using AutoMapper;
using Lima.Sales.Api.Domain;
using Lima.Sales.Api.Repositories;
using Lima.Sales.Api.Requests;
using Lima.Sales.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Sales.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private ISalesRepository _salesRepository;
        private IOrderService _orderService;
        private IMapper _mapper;
        private ILogger _logger;
        private int _userId;

        public SalesController(
            ISalesRepository salesRepository,
            IOrderService orderService,
            IMapper mapper,
            ILogger logger,
            ClaimsPrincipal claimsPrincipal)
        {
            _salesRepository = salesRepository;
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;

            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (userIdClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            _userId = Convert.ToInt32(userIdClaim.Value);
        }

        /// <summary>
        /// Создаёт отгрузку от брони.
        /// </summary>
        /// <param name="salesRequest"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SalesRequest salesRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            NewSale newSale = _mapper.Map<NewSale>(salesRequest);

            _logger.LogInformation($"{salesRequest.Name}, Prepayment {salesRequest.PrepaymentPercent}\n" +
                $"{string.Join("\n", salesRequest.SalesDetailing.Select(s => $"DrugId: {s.DrugId}, Amount: {s.Amount}, Sale price: {s.SalePrice}"))}");

            return Ok(new 
            { 
                SaleId = await _salesRepository.AddSaleAsync(newSale, _userId) 
            });
        }

        /// <summary>
        /// Подтверждает отгрузку.
        /// </summary>
        /// <param name="saleId">Id отгрузки.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut("accept/{saleId}")]
        public async Task<IActionResult> Accept(int saleId)
        {
            var sale = await _salesRepository.GetSaleAsync(saleId);

            if (sale == null)
                throw new Exception("Отгрузка не найдена.");

            if (sale.StatusId == 2)
                throw new Exception("Отгрузка уже подтверждена.");

            if (await _salesRepository.ChangeStatusAsync(saleId, 2))
            {
                await _salesRepository.AssignUserWhoConfirmedTheSale(saleId, _userId);
            }

            return Ok(new 
            { 
                saleId, 
                status = 2 
            });
        }

        /// <summary>
        /// Отменяет отгрузку.
        /// </summary>
        /// <param name="saleId">Id отгрузки.</param>
        /// <returns></returns>
        [HttpPut("cancel/{saleId}")]
        public async Task<IActionResult> Cancel(int saleId)
        {
            return Ok();
        }

        /// <summary>
        /// Получает все продажи.
        /// </summary>
        /// <param name="salesFilterRequest">Фильтр.</param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> All([FromQuery] SalesFilterRequest salesFilterRequest)
        {
            salesFilterRequest = salesFilterRequest ?? new SalesFilterRequest();

            SalesFilter salesFilter = _mapper.Map<SalesFilter>(salesFilterRequest);
            salesFilter.Offset = salesFilterRequest.Page * 10;

            return Ok(await _salesRepository.GetSalesAsync(salesFilter, _userId));
        }

        /// <summary>
        /// Получает детализацию продаж по чеку брони.
        /// </summary>
        /// <param name="orderCheckId"></param>
        /// <returns></returns>
        [HttpGet("detailing/{orderCheckId}")]
        public async Task<IActionResult> SaleDetailing(string orderCheckId)
        {
            var saleDetailing = await _salesRepository.GetSaleDetailingAsync(orderCheckId);

           // var order = await _orderService.GetOrderAsync(orderCheckId, Request.Headers[HeaderNames.Authorization]);

            return Ok(saleDetailing);
        }

        /// <summary>
        /// Получает все предоплаты.
        /// </summary>
        /// <param name="prepaymentsFilterRequest">Фильтр.</param>
        /// <returns></returns>
        [HttpGet("prepayments")]
        public async Task<IActionResult> Prepayments([FromQuery] PrepaymentFilterRequest prepaymentsFilterRequest)
        {
            PrepaymentFilter prepaymentFilter = _mapper.Map<PrepaymentFilter>(prepaymentsFilterRequest);
            prepaymentFilter.Offset = prepaymentsFilterRequest.Page * 10;

            return Ok(await _salesRepository.GetPrepaymentsAsync(prepaymentFilter, _userId));
        }

        /// <summary>
        /// Получает статистику по продажам.
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics()
            => Ok(await _salesRepository.GetSaleStatisticsAsync(_userId));
    }
}
