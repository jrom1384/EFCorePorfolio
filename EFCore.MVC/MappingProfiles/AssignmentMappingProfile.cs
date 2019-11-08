using AutoMapper;
using EFCore.DTO;
using EFCore.MVC.Models;

namespace EFCore.MVC.MappingProfiles
{
    public class AssignmentMappingProfile : Profile
    {
        public AssignmentMappingProfile()
        {
            CreateMap<AssignmentDTO, AssignmentVM>()
                    .ReverseMap();

            CreateMap<AssignmentPageFilterVM, AssignmentPageFilterDTO>();
        }
    }
}
