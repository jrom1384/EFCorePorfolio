using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.MVC.Models;

namespace EFCore.MVC.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<RoleDTO, RoleVM>()
                .ReverseMap();

            CreateMap<GenericPage<RoleVM>, PageFilterDTO>();
        }
    }
}
