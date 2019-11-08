﻿using EFCore.DataLayer;
using EFCore.Razor.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Razor.Configurations
{
    public static class DBContextConfiguration
    {
        public static void AddDBContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.CompanyEFCoreDBContext), b => b.MigrationsAssembly("EFCore.Razor"));
            });
        }
    }
}