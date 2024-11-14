using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.BusSchedule;

namespace pentasharp.Mappings
{
    public class BusScheduleProfile : Profile
    {
        public BusScheduleProfile()
        {
            CreateMap<AddScheduleViewModel, BusSchedule>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}