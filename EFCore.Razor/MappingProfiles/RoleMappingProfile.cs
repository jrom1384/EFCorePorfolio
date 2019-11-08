using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;

namespace EFCore.Razor.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<RoleDTO, RoleModel>()
                .ReverseMap();

            CreateMap<GenericPage<RoleModel>, PageFilterDTO>();
        }
    }
}
