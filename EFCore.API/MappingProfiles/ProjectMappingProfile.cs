using AutoMapper;
using EFCore.API.Models;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<NewProjectRequest, ProjectDTO>();

            CreateMap<ProjectRequest, ProjectDTO>();

            CreateMap<ProjectDTO, ProjectResponse>();
        }
    }
}
