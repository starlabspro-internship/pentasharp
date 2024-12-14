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

            CreateMap<BusReservations, MyBusReservationViewModel>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.ReservationId))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate))
                .ForMember(dest => dest.NumberOfSeats, opt => opt.MapFrom(src => src.NumberOfSeats))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString())) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.BusNumber, opt => opt.MapFrom(src => src.Schedule.Bus.BusNumber))
                .ForMember(dest => dest.FromLocation, opt => opt.MapFrom(src => src.Schedule.Route.FromLocation))
                .ForMember(dest => dest.ToLocation, opt => opt.MapFrom(src => src.Schedule.Route.ToLocation))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Schedule.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.Schedule.ArrivalTime))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Schedule.Price));
        }
    }
}
