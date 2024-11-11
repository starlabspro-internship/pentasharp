using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Bus;

namespace pentasharp.Mappings
{
    public class BusProfile : Profile
    {
        public BusProfile()
        {
            // Mapping for AddBusViewModel
            CreateMap<Buses, AddBusViewModel>()
                .ForMember(dest => dest.BusCompanyId, opt => opt.MapFrom(src => src.BusCompanyId))
                .ReverseMap()
                .ForMember(dest => dest.BusId, opt => opt.Ignore())           // Ignore BusId during creation
                .ForMember(dest => dest.BusCompany, opt => opt.Ignore());     // Ignore BusCompany navigation property if not needed

            // Mapping for EditBusViewModel
            CreateMap<Buses, EditBusViewModel>()
                .ForMember(dest => dest.BusCompanyId, opt => opt.MapFrom(src => src.BusCompanyId))
                .ReverseMap()
                .ForMember(dest => dest.BusId, opt => opt.Ignore())           // Ignore BusId to prevent unintentional changes
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());      // Ignore UpdatedAt if you handle this in code

            // You can add other mappings here if needed, to avoid duplicates.
        }
    }
}