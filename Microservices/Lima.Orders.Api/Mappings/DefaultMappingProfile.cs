using AutoMapper;
using Lima.Orders.Api.Domain;
using Lima.Orders.Api.Requests;

namespace Lima.Orders.Api.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<OrderFilterRequest, OrderFilter>();
            CreateMap<OrderRequest, NewOrder>();
            CreateMap<OrderDetailingRequest, NewOrderDetailing>();
        }
    }
}
