using EFCore.DataLayer;
using EFCore.ServiceLayer;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.API.Configurations
{
    public static class ScopeConfiguration
    {
        public static void AddScopeConfiguration(this IServiceCollection services)
        {
            // Data Access Layer
            services.AddScoped<DataLayer.IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IApiClientRepository, ApiClientRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            //Service Layer
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IApiClientService, ApiClientService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
        }
    }
}
