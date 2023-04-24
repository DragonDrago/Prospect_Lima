using AutoMapper;
using Lima.Incomes.Api.Model;
using Lima.Incomes.Api.Requests;

namespace Lima.Incomes.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<SummaryFilterRequest, IncomeSummaryFilter>();
        }
    }
}
