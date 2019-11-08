using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.MVC.Models;

namespace EFCore.MVC.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<ProjectDTO, ProjectVM>()
                .ReverseMap();

            CreateMap<GenericPage<ProjectVM>, PageFilterDTO>();
        }
    }
}
