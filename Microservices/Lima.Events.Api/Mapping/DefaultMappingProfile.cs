using AutoMapper;
using Lima.Events.Api.Domain;
using Lima.Events.Api.Requests;

namespace Lima.Events.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<EventMapFilterRequest, EventMapFilter>();
        }
    }
}
