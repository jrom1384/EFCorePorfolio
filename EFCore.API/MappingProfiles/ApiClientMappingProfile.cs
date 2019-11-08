using AutoMapper;
using EFCore.API.Models;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class ApiClientMappingProfile : Profile
    {
        public ApiClientMappingProfile()
        {
            CreateMap<ApiClientRegistrationRequest, ApiClientDTO>();

            CreateMap<ApiClientRequest, ApiClientDTO>();

            CreateMap<ApiClientDTO, ApiClientResponse>();

            CreateMap<ApiClientDTO, ApiClient>();
        }
    }
}
