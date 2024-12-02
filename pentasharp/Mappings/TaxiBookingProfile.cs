using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiBooking;

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
                .ForMember(dest => dest.PassengerName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Taxi != null ? src.Taxi.DriverName : "No Driver Chosen"));

            CreateMap<EditTaxiBookingViewModel, TaxiBookings>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}