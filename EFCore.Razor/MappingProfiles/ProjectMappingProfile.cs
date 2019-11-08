using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;

namespace EFCore.Razor.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<ProjectDTO, ProjectModel>()
                .ReverseMap();

            CreateMap<GenericPage<ProjectModel>, PageFilterDTO>();
        }
    }
}
