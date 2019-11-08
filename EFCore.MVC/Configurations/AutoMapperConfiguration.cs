using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.MVC.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles.DepartmentMappingProfile), typeof(ServiceLayer.MappingProfiles.DepartmentMappingProfile));
        }
    }
}
