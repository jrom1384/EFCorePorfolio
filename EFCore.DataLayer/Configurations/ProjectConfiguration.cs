using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Configurations
{
    public static class ProjectConfiguration
    {
        public static void ConfigureProject(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectID);

            modelBuilder.Entity<Project>()
                .HasIndex(c => c.ProjectName)
                .IsUnique();
        }
    }
}
