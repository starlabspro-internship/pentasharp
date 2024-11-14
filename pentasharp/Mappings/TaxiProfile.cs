using AutoMapper;
using pentasharp.Models.Entities;
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
                .ReverseMap()
                .ForMember(dest => dest.TaxiId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
