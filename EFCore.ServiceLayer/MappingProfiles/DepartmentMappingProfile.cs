using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<Department, DepartmentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DepartmentID))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.DepartmentName));

            CreateMap<DepartmentDTO, Department>()
                .ForMember(dest => dest.DepartmentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department));
        }
    }
}
