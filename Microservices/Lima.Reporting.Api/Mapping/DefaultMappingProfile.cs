using AutoMapper;
using Lima.Reporting.Api.Requests;
using Lima.Reporting.Api.Domain;

namespace Lima.Reporting.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<ReportFilterRequest, ReportFilter>();
        }
    }
}
