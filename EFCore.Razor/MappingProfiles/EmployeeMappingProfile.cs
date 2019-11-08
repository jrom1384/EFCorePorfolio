using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;

namespace EFCore.Razor.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<EmployeeDTO, EmployeeModel>()
                .ReverseMap();

            CreateMap<GenericPage<EmployeeModel>, EmployeePageFilterDTO>();
        }
    }
}
