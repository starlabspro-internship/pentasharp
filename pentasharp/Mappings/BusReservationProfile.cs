using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.BusReservation;

namespace pentasharp.Mappings
{
    public class BusReservationProfile : Profile
    {
        public BusReservationProfile()
        {
            CreateMap<AddBusReservationViewModel, BusReservations>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<EditReservationViewModel, BusReservations>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
