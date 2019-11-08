using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;

namespace EFCore.ServiceLayer.MappingProfiles
{
    public class ApiClientMappingProfile : Profile
    {
        public ApiClientMappingProfile()
        {
            CreateMap<ApiClient, ApiClientDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApiClientID));

            CreateMap<ApiClientDTO, ApiClient>()
                .ForMember(dest => dest.ApiClientID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
