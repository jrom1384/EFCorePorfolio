using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Configurations
{
    public static class EmployeeConfiguration
    {
        public static void ConfigureEmployee(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeID);

            modelBuilder.Entity<Employee>()
                .HasIndex(i => new { i.FirstName, i.LastName, i.DateOfBirth })
                .IsUnique();
        }
    }
}
