using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Bus;

namespace pentasharp.Mappings
{
    public class BusCompanyProfile : Profile
    {
        public BusCompanyProfile()
        {
            CreateMap<BusCompany, BusCompanyViewModel>()
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo)) // Assuming ContactNumber is a field in the entity
                .ReverseMap();

            // Mapping for creating a BusCompany from BusCompanyViewModel
            CreateMap<BusCompanyViewModel, BusCompany>()
                .ForMember(dest => dest.BusCompanyId, opt => opt.Ignore()) // Ignore BusCompanyId during creation
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo)); // Map ContactInfo back to ContactNumber
        }
    }
}
