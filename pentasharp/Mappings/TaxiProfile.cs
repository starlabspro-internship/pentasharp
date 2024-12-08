using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.Models.TaxiRequest;
using pentasharp.ViewModel.Authenticate;
using pentasharp.ViewModel.TaxiModels;

namespace pentasharp.Mappings
{
    public class TaxiProfile : Profile
    {
        public TaxiProfile()
        {
            CreateMap<Taxi, AddTaxiViewModel>()
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId))
                .ReverseMap()
                .ForMember(dest => dest.TaxiId, opt => opt.Ignore())
                .ForMember(dest => dest.TaxiCompany, opt => opt.Ignore());

            CreateMap<Taxi, EditTaxiViewModel>()
               .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId))
               .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.DriverId))
               .ReverseMap()
               .ForMember(dest => dest.TaxiId, opt => opt.Ignore())
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.Driver, opt => opt.Ignore());

            CreateMap<AddTaxiDriverViewModel, Taxi>()
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId));

            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));


            CreateMap<Taxi, TaxiRequest>();

        }
    }
}
