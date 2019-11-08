using AutoMapper;
using EFCore.API.Models;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<NewRoleRequest, RoleDTO>();

            CreateMap<RoleRequest, RoleDTO>();

            CreateMap<RoleDTO, RoleResponse>();
        }
    }
}
