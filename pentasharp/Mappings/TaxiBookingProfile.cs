using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Mapping
{
    public class TaxiBookingProfile : Profile
    {
        public TaxiBookingProfile()
        {
            CreateMap<CreateBookingViewModel, TaxiBookings>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending))
                .ForMember(dest => dest.TaxiId, opt => opt.Ignore())
                .ForMember(dest => dest.Notifications, opt => opt.Ignore());

            CreateMap<TaxiBookings, TaxiBookingViewModel>()
                .ForMember(dest => dest.PassengerName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : "Unknown"))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Taxi.Driver != null ? $"{src.Taxi.Driver.FirstName} {src.Taxi.Driver.LastName}" : "No Driver Assigned"))
                .ForMember(dest => dest.BookingTime, opt => opt.MapFrom(src => src.BookingTime.ToString("HH:mm")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<EditTaxiBookingViewModel, TaxiBookings>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}