using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Configurations
{
    public static class RoleConfiguration
    {
        public static void ConfigureRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleID);

            modelBuilder.Entity<Role>()
                .HasIndex(c => c.RoleName)
                .IsUnique();
        }
    }
}
