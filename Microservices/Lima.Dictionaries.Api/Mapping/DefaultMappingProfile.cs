using AutoMapper;
using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.Requests;

namespace Lima.Dictionaries.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<DoctorFilterRequest, DoctorFilter>();
            CreateMap<DistributorFilterRequest, DistributorFilter>();
            CreateMap<DoctorRequest, Doctor>();
            CreateMap<DrugRequest, Drug>();
            CreateMap<DrugFilterRequest, DrugFilter>();
            CreateMap<HealthCareFacilityFilterRequest, HealthCareFacilityFilter>();
            CreateMap<OrganizationFilterRequest, PharmacyFilter>();
            CreateMap<OrganizationRequest, Organization>();
            CreateMap<EntityRequest, EntityBase>();
            CreateMap<DrugSeriesRequest, DrugSeries>();
        }
    }
}
