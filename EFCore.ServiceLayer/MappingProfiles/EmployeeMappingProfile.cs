using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeID))
                 .ForMember(dest => dest.Department_Id, opt => opt.MapFrom(src => src.DepartmentID))
                 .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.DepartmentName));

            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.EmployeeID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentID, opt => opt.MapFrom(src => src.Department_Id))
                .ForMember(dest => dest.Department, opt => opt.Ignore());
        }
    }
}
