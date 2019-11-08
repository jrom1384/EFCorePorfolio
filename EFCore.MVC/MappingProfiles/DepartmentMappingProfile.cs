using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.MVC.Models;

namespace EFCore.MVC.MappingProfiles
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<DepartmentDTO, DepartmentVM>()
                .ReverseMap();

            CreateMap<GenericPage<DepartmentVM>, PageFilterDTO>();
        }
    }
}
