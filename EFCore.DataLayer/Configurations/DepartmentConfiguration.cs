using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Configurations
{
    public static class DepartmentConfiguration
    {
        public static void ConfigureDepartment(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasKey(d => d.DepartmentID);

            modelBuilder.Entity<Department>()
                .HasIndex(c => c.DepartmentName)
                .IsUnique();

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
