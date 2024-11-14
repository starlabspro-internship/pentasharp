using AutoMapper;
using pentasharp.Models.Entities;
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
        }
    }
}
