using AutoMapper;
using EFCore.DTO;
using EFCore.MVC.Models;

namespace EFCore.MVC.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<RegisterUserVM, RegisterUserDTO>();

            CreateMap<ApplicationUserVM, ApplicationUserDTO>()
                .ReverseMap();
        }
    }
}
