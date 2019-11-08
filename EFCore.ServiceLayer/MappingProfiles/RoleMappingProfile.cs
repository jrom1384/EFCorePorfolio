using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<RoleDTO, Role>()
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role));
        }
    }
}
