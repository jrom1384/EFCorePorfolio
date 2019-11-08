using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProjectID))
               .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.ProjectName));

            CreateMap<ProjectDTO, Project>()
               .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project));
        }
    }
}
