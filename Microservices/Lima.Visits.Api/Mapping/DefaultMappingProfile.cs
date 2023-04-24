using AutoMapper;
using Lima.Visits.Api.Domain;
using Lima.Visits.Api.Requests;

namespace Lima.Visits.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<VisitFilterRequest, VisitFilter>();
            CreateMap<PharmacyDistributorVisitRequest, PharmacyDistributorVisit>();
            CreateMap<NewVisitRequest, Visit>();
            CreateMap<DrugRequest, SelectedDrug>();
            CreateMap<DrugBalanceRequest, Balance>();
        }
    }
}
