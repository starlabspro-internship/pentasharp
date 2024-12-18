using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.BusSchedul;

namespace pentasharp.Mappings
{
    public class BusScheduleProfile : Profile
    {
        public BusScheduleProfile()
        {
            CreateMap<AddScheduleViewModel, BusSchedule>()
                .ForMember(dest => dest.ScheduleId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<BusSchedule, AddScheduleViewModel>();

            CreateMap<BusSchedule, SearchScheduleViewModel>()
                .ForMember(dest => dest.FromLocation, opt => opt.MapFrom(src => src.Route.FromLocation))
                .ForMember(dest => dest.ToLocation, opt => opt.MapFrom(src => src.Route.ToLocation))
                .ForMember(dest => dest.BusNumber, opt => opt.MapFrom(src => src.Bus.BusNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}