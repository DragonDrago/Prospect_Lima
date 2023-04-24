using Dapper;
using Lima.Core.Tenant;
using Lima.Events.Api.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Events.Api.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private ITenantContext _tenantContext;

        public EventsRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<int> AddOrUpdateEvent(Event evt, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int eventId = await connection.QueryFirstOrDefaultAsync<int>(
                       sql: "dbo.insertOrUpdateEvent",
                       param: new
                       {
                           @id = evt.Id,
                           @organizationId = evt.OrganizationId,
                           @visitId = evt.VisitId,
                           @startDate = evt.StartDate,
                           @endDate = evt.EndDate,
                           @comment = evt.Comment,
                           @isTask = evt.IsTask,
                           @repeatDays = evt.RepeatDays,
                           @colorId = evt.Color.ColorId,
                           @userId
                       },
                       commandType: CommandType.StoredProcedure,
                       transaction: transaction);

                        if (evt.EventDoctor != null)
                        {
                            await connection.QueryAsync(sql: @"INSERT INTO EVENTS_DOCTOR (EventId, DoctorName, DoctorPosition, DoctorPhone)
                                                        VALUES (@eventId, @doctorName, @doctorPosition, @doctorPhone)",
                                                    param: new
                                                    {
                                                        @eventId,
                                                        @doctorName = evt.EventDoctor.DoctorName,
                                                        @doctorPosition = evt.EventDoctor.DoctorPosition,
                                                        @doctorPhone = evt.EventDoctor.DoctorPhone
                                                    },
                                                    transaction: transaction);
                        }

                        if (evt.EventMembers != null)
                        {
                            foreach (var memberItem in evt.EventMembers)
                            {
                                await connection.QueryAsync(sql: @"INSERT INTO EVENTS_MEMBER(EventId, UserId)
                                                        VALUES (@eventId, @userId)",
                                                    param: new
                                                    {
                                                        @eventId,
                                                        @userId = memberItem.MemberUserId
                                                    },
                                                    transaction: transaction);
                            }
                        }

                        if (evt.TalkedAboutTheDrugs != null)
                        {
                            foreach (var drugItem in evt.TalkedAboutTheDrugs)
                            {
                                await connection.QueryAsync(sql: @"INSERT INTO EVENTS_TALK_ABOUT_DRUGS(EventId, DrugId)
                                                        VALUES (@eventId, @drugId)",
                                                    param: new
                                                    {
                                                        @eventId,
                                                        @drugId = drugItem.DrugId
                                                    },
                                                    transaction: transaction);
                            }
                        }


                        await transaction.CommitAsync();

                        return eventId;
                    }
                    catch (System.Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<EventMap>> GetEventMapAsync(EventMapFilter eventMapFilter, int userId)
        {
            DateTime? startDate = eventMapFilter.Dates != null && eventMapFilter.Dates.Count() == 2 ? 
                eventMapFilter.Dates[0] : null;

            DateTime? endDate = eventMapFilter.Dates != null && eventMapFilter.Dates.Count() == 2 ? 
                eventMapFilter.Dates[1].AddHours(23).AddMinutes(59).AddSeconds(59) : null;

            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<EventMap, Organization, Visit, EventMap>(
                    sql: "dbo.getEventMap",
                    param: new { @userId, @startDate = startDate, @endDate = endDate },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "OrganizationId,VisitId",
                    map: (em, o, v) => 
                    {
                        em.Organization = o;
                        em.Visit = v;

                        return em;
                    });

                if (eventMapFilter != null)
                {
                    if (eventMapFilter.UserIds != null && eventMapFilter.UserIds.Length > 0)
                    {
                        result = result.Where(w => eventMapFilter.UserIds.Contains(w.UserId));
                    }

                    if (eventMapFilter.Dates != null && eventMapFilter.Dates.Length == 2)
                    {
                        result = result.Where(w => w.StartDate >= eventMapFilter.Dates[0] && w.EndDate <= eventMapFilter.Dates[2]);
                    }
                }

                return result;
            }
        }

        public async Task<IEnumerable<Event>> GetEvents(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<Event> events = new List<Event>();

                await connection.QueryAsync<Event, EventColor, EventDoctor, EventMember, EventDrug, Event>(
                    sql: "dbo.getEvents",
                    param: new { @userId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id,ColorId,DoctorId,MemberUserId,DrugId",
                    map: (e, c, d, m, dr) =>
                    {
                        int eventIndex = events.FindIndex(f => f.Id == e.Id);

                        if (eventIndex == -1)
                        {
                            events.Add(e);
                            eventIndex = events.Count - 1;

                            events[eventIndex].Color = c;
                            events[eventIndex].EventDoctor = d;
                        }

                        if (m != null)
                        {
                            int memberIndex = events[eventIndex].EventMembers.FindIndex(f => f.MemberUserId == m.MemberUserId);

                            if (memberIndex == -1)
                                events[eventIndex].EventMembers.Add(m);
                        }

                        if (dr != null)
                        {
                            int drugIndex = events[eventIndex].TalkedAboutTheDrugs.FindIndex(f => f.DrugId == dr.DrugId);

                            if (drugIndex == -1)
                                events[eventIndex].TalkedAboutTheDrugs.Add(dr);
                        }

                        return e;
                    });

                return events;
            }
        }
    }
}
