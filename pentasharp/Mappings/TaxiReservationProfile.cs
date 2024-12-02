using AutoMapper;
using pentasharp.Models.DTOs;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.ViewModel.TaxiReservation;

namespace pentasharp.Mappings
{
    public class TaxiReservationProfile : Profile
    {
        public TaxiReservationProfile()
        {
            CreateMap<TaxiCompany, TaxiCompanyDto>();

            CreateMap<TaxiReservationViewModel, TaxiReservations>()
            .ForMember(dest => dest.ReservationTime, opt => opt.Ignore());

            CreateMap<TaxiReservations, TaxiReservationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))  
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)) 
            .ForMember(dest => dest.ReservationTime, opt => opt.MapFrom(src => src.ReservationTime.ToString("hh:mm tt")))
            .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationTime.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.PassengerCount, opt => opt.MapFrom(src => src.PassengerCount));

            CreateMap<UpdateReservationViewModel, TaxiReservations>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ReservationStatus>(src.Status ?? "Pending")));
        }
    }
}
