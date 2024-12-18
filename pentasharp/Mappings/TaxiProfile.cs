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
            CreateMap<Taxi, AddTaxiRequest>()
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId))
                .ReverseMap()
                .ForMember(dest => dest.TaxiId, opt => opt.Ignore())
                .ForMember(dest => dest.TaxiCompany, opt => opt.Ignore());

            CreateMap<Taxi, EditTaxiRequest>()
               .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId))
               .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.DriverId))
               .ReverseMap()
               .ForMember(dest => dest.TaxiId, opt => opt.Ignore())
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.Driver, opt => opt.Ignore());

            CreateMap<AddTaxiDriverRequest, Taxi>()
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId));

            CreateMap<RegisterDriverRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Taxi, TaxiRequest>()
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null
                      ? $"{src.Driver.FirstName} {src.Driver.LastName}"
                      : "Unassigned"))
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.MapFrom(src => src.TaxiCompanyId))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.TaxiCompany.CompanyName));
        }
    }
}