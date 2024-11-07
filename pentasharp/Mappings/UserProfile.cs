using AutoMapper;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Authenticate;

namespace pentasharp.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map User entity to RegisterViewModel and EditUserViewModel
            CreateMap<User, RegisterViewModel>().ReverseMap();
            CreateMap<User, EditUserViewModel>().ReverseMap();
        }
    }
}