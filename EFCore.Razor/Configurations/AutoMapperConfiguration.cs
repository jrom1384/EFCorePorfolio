using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Razor.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceLayer.MappingProfiles.DepartmentMappingProfile), typeof(MappingProfiles.DepartmentMappingProfile));
        }
    }
}
