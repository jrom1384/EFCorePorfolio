using AutoMapper;
using EFCore.Common;
using EFCore.DTO;
using EFCore.Razor.Models;

namespace EFCore.Razor.MappingProfiles
{
    public class AssigmentMappingProfile : Profile
    {
        public AssigmentMappingProfile()
        {
            CreateMap<AssignmentDTO, AssignmentModel>()
                    .ReverseMap();

            CreateMap<GenericPage<AssignmentModel>, AssignmentPageFilterDTO>();
        }
    }
}
