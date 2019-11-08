using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Seeds
{
    public static class DeparmentSeeder
    {
        public static void SeedDepartment(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    DepartmentID = 1,
                    DepartmentName = "IT Development (RnD)"
                },
                new Department
                {
                    DepartmentID = 2,
                    DepartmentName = "Human Resource"
                },
                new Department
                {
                    DepartmentID = 3,
                    DepartmentName = "Accounting and Finance"
                },
                new Department
                {
                    DepartmentID = 4,
                    DepartmentName = "Training and Certifications"
                },
                new Department
                {
                    DepartmentID = 5,
                    DepartmentName = "IT Network"
                },
                new Department
                {
                    DepartmentID = 6,
                    DepartmentName = "1st Bussiness Unit"
                },
                new Department
                {
                    DepartmentID = 7,
                    DepartmentName = "Marketing and Advertising"
                },
                new Department
                {
                    DepartmentID = 8,
                    DepartmentName = "Customer Service"
                },
                new Department
                {
                    DepartmentID = 9,
                    DepartmentName = "Utility"
                },
                new Department
                {
                    DepartmentID = 10,
                    DepartmentName = "Security"
                },
                new Department
                {
                    DepartmentID = 11,
                    DepartmentName = "Logistics"
                },
                new Department
                {
                    DepartmentID = 12,
                    DepartmentName = "Deposit Team"
                },
                new Department
                {
                    DepartmentID = 13,
                    DepartmentName = "WD Team"
                },
                new Department
                {
                    DepartmentID = 14,
                    DepartmentName = "Team Pacquaio"
                },
                new Department
                {
                    DepartmentID = 15,
                    DepartmentName = "Sound Tech"
                },
                new Department
                {
                    DepartmentID = 16,
                    DepartmentName = "Design Team"
                },
                new Department
                {
                    DepartmentID = 17,
                    DepartmentName = "Communication"
                }
             );
        }
    }
}
