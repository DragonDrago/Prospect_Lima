using AutoMapper;
using Lima.Core.Rest;
using Lima.Events.Api.Domain;
using Lima.Events.Api.Dto;
using Lima.Events.Api.Repositories;
using Lima.Events.Api.Requests;
using Lima.Events.Api.Services;
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

namespace Lima.Events.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private IEventsRepository _eventsRepository;
        private IVisitService _visitService;
        private IMapper _mapper;
        private ILogger _logger;
        private int _userId;

        public EventsController
            (IEventsRepository eventsRepository, 
            IVisitService visitService, 
            IMapper mapper, 
            ILogger logger,
            ClaimsPrincipal claimsPrincipal)
        {
            _eventsRepository = eventsRepository;
            _visitService = visitService;
            _mapper = mapper;
            _logger = logger;

            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (userIdClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }
            else
            {
                _userId = Convert.ToInt32(userIdClaim.Value);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddEvent([FromBody] EventRequest eventRequest)
        {
            if (eventRequest == null)
                return BadRequest("Невалидный объект.");

            if (!ModelState.IsValid)
            {
                throw new System.Exception(string.Join("\n", ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage))));
            }

            Event newEvent = new Event();
            newEvent.Id = eventRequest.Id;
            newEvent.OrganizationId = eventRequest.OrganizationId;
            newEvent.StartDate = eventRequest.StartDate;
            newEvent.EndDate = eventRequest.EndDate;
            newEvent.Comment = eventRequest.Comment;
            newEvent.IsTask = eventRequest.IsTask;
            newEvent.RepeatDays = eventRequest.RepeatDays;
            newEvent.Color = new EventColor() { ColorId = eventRequest.ColorId };
            newEvent.EventDoctor = !string.IsNullOrEmpty(eventRequest.DoctorName?.Trim()) ? new EventDoctor()
            { 
                DoctorName = eventRequest.DoctorName,
                DoctorPhone = eventRequest.DoctorPhone,
                DoctorPosition = eventRequest.DoctorPosition,
            } : null;

            newEvent.EventMembers = eventRequest.Members != null ? eventRequest.Members.Select(s => new EventMember() { MemberUserId = s }).ToList() : null;
            newEvent.TalkedAboutTheDrugs = eventRequest.Drugs != null ? eventRequest.Drugs.Select(s => new EventDrug() { DrugId = s }).ToList() : null;

            //создание визита
            newEvent.VisitId = await CreateVisit(newEvent, eventRequest.TypeId);
            
            //создание события
            int eventId = await _eventsRepository.AddOrUpdateEvent(newEvent, _userId);

            return Ok(eventId);
        }

        [HttpGet("map")]
        public async Task<IActionResult> GetEvents([FromQuery] EventMapFilterRequest eventFilterRequest)
        {
            EventMapFilter eventMapFilter = null;

            if (eventFilterRequest != null)
                eventMapFilter = _mapper.Map<EventMapFilter>(eventFilterRequest);

            return Ok(await _eventsRepository.GetEventMapAsync(eventMapFilter, _userId));
        }

        private async Task<int?> CreateVisit(Event @event, int? typeId)
        {
            NewVisitDto newVisitDto = new NewVisitDto()
            {
                Comment = @event.Comment,
                DoctorName = @event.EventDoctor?.DoctorName,
                DoctorPhone = @event.EventDoctor?.DoctorPhone,
                DoctorPosition = @event.EventDoctor?.DoctorPosition,
                OrganizationId = @event.OrganizationId,
                OrganizationName = @event.OrganizationName
            };

            try
            {
                if (typeId == 1)
                {
                    return await _visitService.AddVisitToPharmacyAsync(newVisitDto, Request.Headers[HeaderNames.Authorization]);
                }
                else if (typeId == 2)
                {
                    return await _visitService.AddVisitToDoctorAsync(newVisitDto, Request.Headers[HeaderNames.Authorization]);
                }
                else if (typeId == 3)
                {
                   return await _visitService.AddVisitToDistributorAsync(newVisitDto, Request.Headers[HeaderNames.Authorization]);
                }
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var badRequestResponse = ex.DeserializeContent<ResponseErrorMessage>();
                throw new Exception(badRequestResponse.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(EventsController));
                throw new Exception("Не удалось создать визит. Обратитесь к администратору.");
            }

            return null;
        }
    }
}
