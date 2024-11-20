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
        }
    }
}