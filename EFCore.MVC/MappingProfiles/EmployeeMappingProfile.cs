using AutoMapper;
using EFCore.DTO;
using EFCore.MVC.Models;

namespace EFCore.MVC.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<EmployeeDTO, EmployeeVM>()
                .ReverseMap();

            CreateMap<EmployeePageFilterVM, EmployeePageFilterDTO>();
        }
    }
}
