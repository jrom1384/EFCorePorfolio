using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class AssignmentMappingProfile : Profile
    {
        public AssignmentMappingProfile()
        {
            CreateMap<Assignment, AssignmentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssignmentID))
                .ForMember(dest => dest.Project_Id, opt => opt.MapFrom(src => src.ProjectID))
                .ForMember(dest => dest.Employee_Id, opt => opt.MapFrom(src => src.EmployeeID))
                .ForMember(dest => dest.Role_Id, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project.ProjectName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Employee.Department.DepartmentName));

            CreateMap<AssignmentDTO, Assignment>()
                .ForMember(dest => dest.AssignmentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.Project_Id))
                .ForMember(dest => dest.EmployeeID, opt => opt.MapFrom(src => src.Employee_Id))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.Role_Id))
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());
        }
    }
}
