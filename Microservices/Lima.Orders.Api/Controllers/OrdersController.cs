using AutoMapper;
using Lima.Core.Rest;
using Lima.Orders.Api.Domain;
using Lima.Orders.Api.Dto;
using Lima.Orders.Api.Repositories;
using Lima.Orders.Api.Requests;
using Lima.Orders.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RestEase;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Orders.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IOrderRepository _orderRepository;
        private ISalesService _salesService;
        private IMapper _mapper;
        private ILogger _logger;
        private int _userId;

        public OrdersController(
            IOrderRepository orderRepository, 
            ISalesService salesService,
            IMapper mapper,
            ILogger logger,
            ClaimsPrincipal claimsPrincipal)
        {
            _mapper = mapper;
            _logger = logger;
            _orderRepository = orderRepository;
            _salesService = salesService;

            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (userIdClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }

            _userId = Convert.ToInt32(userIdClaim.Value);
        }

        /// <summary>
        /// Осуществляет поиск брони по Id или чеку.
        /// </summary>
        /// <param name="checkOrId"></param>
        /// <returns></returns>
        [HttpGet("find/{checkOrId}")]
        public async Task<IActionResult> Find(string checkOrId)
            => Ok(await _orderRepository.GetOrderAsync(checkOrId));

        /// <summary>
        /// Подтверждает бронь.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("accept/{orderId}")]
        public async Task<IActionResult> Accept(int orderId)
        {
            if (orderId <= 0)
                throw new Exception("Неверный ID брони.");

            Order order = await _orderRepository.GetOrderAsync($"{orderId}");

            try
            {
                
                if (order.StatusId == 3)
                    throw new Exception("Невозможно подтвердить бронь. Бронь просрочена.");

                if (await _orderRepository.ChangeOrderStatusAsync(orderId, 2))
                {
                    
                    NewSaleDto newSaleDto = new NewSaleDto()
                    {

                        Name = $"От {DateTime.Now}. Бронь № {orderId}",
                        OrderId = orderId,
                        PrepaymentPercent = order.PrepaymentPercent,
                        SalesDetailing = order.OrderDetailings.Select(s => new SaleDetailingDto()
                        {
                            DrugId = s.DrugId,
                            Amount = s.Amount,
                            SalePrice = s.SalePrice
                        }).ToList()
                    };

                    var saleResult = await _salesService.CreateSaleAsync(newSaleDto, Request.Headers[HeaderNames.Authorization]);

                    if (saleResult != null && saleResult.SaleId > 0)
                    {
                        return Ok(new
                        {
                            OrderId = orderId,
                            SaleId = saleResult.SaleId
                        });
                    }
                    else
                    {
                        throw new Exception("Не удалось создать отгрузку. Сервис продаж вернул некорректные данные.");
                    }
                }
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var badRequestResponse = ex.DeserializeContent<ResponseErrorMessage>();
                throw new Exception(badRequestResponse.Message);
            }
            catch (Exception ex)
            {
                if (order.StatusId != 3)
                    await _orderRepository.ChangeOrderStatusAsync(orderId, 1);

                throw;//new Exception("Не удалось создать отгрузку. Обратитесь к администратору.");
            }

            throw new Exception("Что-то пошло не так. Обратитесь к администратору");
        }

        /// <summary>
        /// Создаёт новую бронь.
        /// </summary>
        /// <param name="orderRequest">Объект брони.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrderRequest orderRequest)
        {

            if (orderRequest.OrdersDetailing.Any(a => a.Amount == 0))
                throw new Exception("У одной или нескольких позиций не указано количество.");

            if (orderRequest.OrdersDetailing.Any(a => a.DrugId == 0))
                throw new Exception("Неизвестный ID прихода.");

            NewOrder newOrder = _mapper.Map<NewOrder>(orderRequest);

            return Ok(await _orderRepository.CreateOrUpdateAsync(newOrder));
        }

        /// <summary>
        /// Получает список всех броней.
        /// </summary>
        /// <param name="orderFilterRequest"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpGet("all")]
        public async Task<IActionResult> All([FromQuery] OrderFilterRequest orderFilterRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            orderFilterRequest = orderFilterRequest ?? new OrderFilterRequest() { Page = 1 };

            OrderFilter orderFilter = _mapper.Map<OrderFilter>(orderFilterRequest);
            orderFilter.Offset = orderFilterRequest.Page * 10;

            return Ok(await _orderRepository.All(orderFilter, _userId));
        }

        /// <summary>
        /// Получает статистику по броням.
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics() 
            => Ok(await _orderRepository.GetOrdersStatisticsAsync(_userId));

        /// <summary>
        /// Обновляет существующую бронь.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] OrderRequest orderRequest) 
            => await Create(orderRequest);
    }
}
