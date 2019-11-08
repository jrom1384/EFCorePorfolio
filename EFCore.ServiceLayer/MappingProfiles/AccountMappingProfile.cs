using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
        }
    }
}
