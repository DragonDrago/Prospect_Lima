using AutoMapper;
using Lima.Sales.Api.Domain;
using Lima.Sales.Api.Requests;

namespace Lima.Sales.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<PrepaymentFilterRequest, PrepaymentFilter>();
            CreateMap<SalesFilterRequest, SalesFilter>();
            CreateMap<SalesRequest, NewSale>();
            CreateMap<SalesDetailingRequest, SaleDetailing>();
        }
    }
}
