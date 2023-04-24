using Lima.Events.Api.Domain;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Events.Api.Repositories
{
    public interface IEventsRepository
    {
        Task<IEnumerable<Event>> GetEvents(int userId);
        Task<IEnumerable<EventMap>> GetEventMapAsync(EventMapFilter eventMapFilter, int userId);
        Task<int> AddOrUpdateEvent(Event evt, int userId);
    }
}
