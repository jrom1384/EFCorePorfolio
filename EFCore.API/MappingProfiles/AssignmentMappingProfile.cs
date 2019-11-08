using AutoMapper;
using EFCore.API.Models;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class AssignmentMappingProfile : Profile
    {
        public AssignmentMappingProfile()
        {
            CreateMap<NewAssignmentRequest, AssignmentDTO>();

            CreateMap<AssignmentRequest, AssignmentDTO>();

            CreateMap<AssignmentDTO, AssignmentResponse>();
        }
    }
}
