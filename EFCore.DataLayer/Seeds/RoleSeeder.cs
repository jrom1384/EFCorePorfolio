using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Seeds
{
    public static class RoleSeeder
    {
        public static void SeedRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleID = 1,
                RoleName = "CEO"
            },
            new Role
            {
                RoleID = 2,
                RoleName = "Manager"
            },
            new Role
            {
                RoleID = 3,
                RoleName = "Assistant Manager"
            },
            new Role
            {
                RoleID = 4,
                RoleName = "Software Engineer"
            },
            new Role
            {
                RoleID = 5,
                RoleName = "Probitionary"
            },
            new Role
            {
                RoleID = 6,
                RoleName = "Technical Support"
            },
            new Role
            {
                RoleID = 7,
                RoleName = "Network Engineer"
            },
            new Role
            {
                RoleID = 8,
                RoleName = "Business Analyst"
            },
            new Role
            {
                RoleID = 9,
                RoleName = "Database Administrator"
            },
            new Role
            {
                RoleID = 10,
                RoleName = "Software Tester"
            },
            new Role
            {
                RoleID = 11,
                RoleName = "Sales Agent"
            },
            new Role
            {
                RoleID = 12,
                RoleName = "Team Leader"
            });
        }
    }
}
