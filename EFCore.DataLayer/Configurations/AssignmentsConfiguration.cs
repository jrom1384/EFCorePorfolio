using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Configurations
{
    public static class AssignmentsConfiguration
    {
        public static void ConfigureAssignment(this ModelBuilder modelBuilder)
        {
            //Bridge Table
            modelBuilder.Entity<Assignment>()
                .HasKey(k => k.AssignmentID);

            modelBuilder.Entity<Assignment>()
                .HasIndex(k => new { k.ProjectID, k.EmployeeID })
                .IsUnique();

            modelBuilder.Entity<Assignment>()
                .HasOne(k => k.Project)
                .WithMany(k => k.Assignments)
                .HasForeignKey(k => k.ProjectID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assignment>()
                .HasOne(k => k.Employee)
                .WithMany(m => m.Assignments)
                .HasForeignKey(k => k.EmployeeID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assignment>()
                .HasOne(k => k.Role)
                .WithMany(k => k.Assignments)
                .HasForeignKey(k => k.RoleID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
