using AutoMapper;
using EFCore.API.Models;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<NewDepartmentRequest, DepartmentDTO>();

            CreateMap<DepartmentRequest, DepartmentDTO>();

            CreateMap<DepartmentDTO, DepartmentResponse>();
        }
    }
}
