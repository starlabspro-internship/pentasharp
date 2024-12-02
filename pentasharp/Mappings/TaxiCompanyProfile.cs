using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.Taxi;
using pentasharp.ViewModel.TaxiModels;

namespace pentasharp.Mappings
{
    public class TaxiCompanyProfile : Profile
    {
        public TaxiCompanyProfile()
        {
            CreateMap<TaxiCompany, TaxiCompanyViewModel>()
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ReverseMap()
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.Ignore());

            CreateMap<TaxiCompanyViewModel, TaxiCompany>()
                .ForMember(dest => dest.TaxiCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo));

            CreateMap<Taxi, TaxiViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) 
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaxiStatus>(src.Status, true))); 

            CreateMap<AddTaxiViewModel, Taxi>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaxiStatus>(src.Status, true)));

            CreateMap<EditTaxiViewModel, Taxi>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaxiStatus>(src.Status, true)));

        }
    }
}
