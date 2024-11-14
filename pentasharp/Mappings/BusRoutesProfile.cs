using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.BusSchedule;

namespace pentasharp.Mappings
{
    public class BusRoutesProfile : Profile
    {
        public BusRoutesProfile()
        {
            CreateMap<AddRouteViewModel, BusRoutes>()
                .ForMember(dest => dest.RouteId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<BusRoutes, AddRouteViewModel>();
        }
    }
}