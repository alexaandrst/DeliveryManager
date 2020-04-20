using AutoMapper;
using DeliveryManager.Models;
using DeliveryManager.Models.Requests;

namespace DeliveryManager
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateWindowRequest, DeliveryWindow>();
            CreateMap<UpdateWindowRequest, DeliveryWindow>();

            CreateMap<DeliveryWindowDto, DeliveryWindow>();
            CreateMap<DeliveryWindow, DeliveryWindowDto>();;
        }
    }
}
