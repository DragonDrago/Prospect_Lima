using AutoMapper;
using Lima.Admin.Api.Domain;
using Lima.Admin.Api.Requests;

namespace Lima.Admin.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<BranchRequest, Branch>();
            CreateMap<CompanyRequest, Company>();
            CreateMap<DepartmentRequest, Department>();
            CreateMap<MarkupRequest, Markup>();
            CreateMap<UserRequest, User>();
            CreateMap<EntityRequest, Entity>();
        }
    }
}
