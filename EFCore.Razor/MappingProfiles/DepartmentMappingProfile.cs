using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;

namespace EFCore.Razor.MappingProfiles
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<DepartmentDTO, DepartmentModel>()
                .ReverseMap();

            CreateMap<GenericPage<DepartmentModel>, PageFilterDTO>();
        }
    }
}
