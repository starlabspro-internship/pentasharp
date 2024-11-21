using AutoMapper;
using pentasharp.Migrations;
using pentasharp.Models.Entities; // Adjust namespace for the actual entity
using pentasharp.ViewModel.TaxiReservation;

namespace pentasharp.Mappings
{
    public class TaxiReservationProfile : Profile
    {
        public TaxiReservationProfile()
        {
            CreateMap<TaxiReservations, TaxiSearchViewModel>()
                .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation))
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.DropoffLocation))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationTime))
                .ForMember(dest => dest.ReservationTime, opt => opt.MapFrom(src => src.ReservationTime))
                .ForMember(dest => dest.PassengerCount, opt => opt.MapFrom(src => src.PassengerCount));

            CreateMap<TaxiSearchViewModel, TaxiReservations>()
                .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation))
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.DropoffLocation))
                .ForMember(dest => dest.ReservationTime, opt => opt.MapFrom(src => src.ReservationDate))
                .ForMember(dest => dest.ReservationTime, opt => opt.MapFrom(src => src.ReservationTime))
                .ForMember(dest => dest.PassengerCount, opt => opt.MapFrom(src => src.PassengerCount));
        }
    }
}
