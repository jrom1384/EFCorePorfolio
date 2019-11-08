using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.API.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles.ApiClientMappingProfile), typeof(ServiceLayer.MappingProfiles.ApiClientMappingProfile));
        }
    }
}
