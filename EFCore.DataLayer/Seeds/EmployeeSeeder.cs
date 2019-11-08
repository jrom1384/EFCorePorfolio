using EFCore.Common;
using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore.DataLayer.Seeds
{
    public static class EmployeeSeeder
    {
        public static void SeedEmployee(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData
                (
                    new Employee
                    {
                        EmployeeID = 1,
                        FirstName = "Jerome",
                        LastName = "Bautista",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1984, 1, 13),
                        IsActive = true,
                        DepartmentID = 1
                    },
                    new Employee
                    {
                        EmployeeID = 2,
                        FirstName = "Bruce",
                        LastName = "Wayne",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1980, 6, 12),
                        IsActive = true,
                        DepartmentID = 2
                    },
                    new Employee
                    {
                        EmployeeID = 3,
                        FirstName = "Daenerys",
                        LastName = "Targaryen",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1996, 3, 16),
                        IsActive = true,
                        DepartmentID = 3
                    },
                    new Employee
                    {
                        EmployeeID = 4,
                        FirstName = "James (Logan)",
                        LastName = "Howlett",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1974, 10, 1),
                        IsActive = false,
                        DepartmentID = 4
                    },
                    new Employee
                    {
                        EmployeeID = 5,
                        FirstName = "Rodrigo Roa",
                        LastName = "Duterte",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1945, 3, 28),
                        IsActive = true,
                        DepartmentID = 5
                    },
                    new Employee
                    {
                        EmployeeID = 6,
                        FirstName = "Yeng",
                        LastName = "Constantino",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1988, 12, 4),
                        IsActive = true,
                        DepartmentID = 6
                    },
                    new Employee
                    {
                        EmployeeID = 7,
                        FirstName = "Kitchie",
                        LastName = "Nadal",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1980, 10, 16),
                        IsActive = true,
                        DepartmentID = 7
                    },
                    new Employee
                    {
                        EmployeeID = 8,
                        FirstName = "Manny",
                        LastName = "Pacquiao",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1978, 12, 17),
                        IsActive = true,
                        DepartmentID = 8
                    },
                    new Employee
                    {
                        EmployeeID = 9,
                        FirstName = "Floyd",
                        LastName = "Mayweater",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1977, 2, 24),
                        IsActive = true,
                        DepartmentID = 9
                    },
                    new Employee
                    {
                        EmployeeID = 10,
                        FirstName = "Eduard",
                        LastName = "Folayang",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1984, 10, 22),
                        IsActive = true,
                        DepartmentID = 10
                    },
                    new Employee
                    {
                        EmployeeID = 11,
                        FirstName = "Ronda",
                        LastName = "Rousey",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1987, 2, 1),
                        IsActive = true,
                        DepartmentID = 11
                    },
                    new Employee
                    {
                        EmployeeID = 12,
                        FirstName = "Jose",
                        LastName = "Rizal",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1861, 6, 19),
                        IsActive = true,
                        DepartmentID = 12
                    },
                    new Employee
                    {
                        EmployeeID = 13,
                        FirstName = "Ely",
                        LastName = "Buendia",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1970, 11, 2),
                        IsActive = true,
                        DepartmentID = 13
                    },
                    new Employee
                    {
                        EmployeeID = 14,
                        FirstName = "Chito",
                        LastName = "Miranda",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1976, 2, 7),
                        IsActive = true,
                        DepartmentID = 14
                    },
                    new Employee
                    {
                        EmployeeID = 15,
                        FirstName = "Zsa Zsa",
                        LastName = "Padilla",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1964, 5, 28),
                        IsActive = true,
                        DepartmentID = 15
                    },
                    new Employee
                    {
                        EmployeeID = 16,
                        FirstName = "Kyoko",
                        LastName = "Fukada",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1982, 11, 2),
                        IsActive = true,
                        DepartmentID = 16
                    },
                    new Employee
                    {
                        EmployeeID = 17,
                        FirstName = "Uchu",
                        LastName = "Keiji Shaida",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1984, 3, 2),
                        IsActive = true,
                        DepartmentID = 2
                    },
                    new Employee
                    {
                        EmployeeID = 18,
                        FirstName = "Raffy",
                        LastName = "Tulfo",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1960, 3, 12),
                        IsActive = true,
                        DepartmentID = 1
                    },
                    new Employee
                    {
                        EmployeeID = 19,
                        FirstName = "Nino",
                        LastName = "Schurter",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1980, 8, 20),
                        IsActive = true,
                        DepartmentID = 3
                    },
                    new Employee
                    {
                        EmployeeID = 20,
                        FirstName = "Julien",
                        LastName = "Absalon",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1996, 7, 23),
                        IsActive = true,
                        DepartmentID = 4
                    },
                    new Employee
                    {
                        EmployeeID = 21,
                        FirstName = "Mary Jane",
                        LastName = "Nunez",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1999, 3, 16),
                        IsActive = true,
                        DepartmentID = 5
                    },
                    new Employee
                    {
                        EmployeeID = 22,
                        FirstName = "Yael",
                        LastName = "Yuzon",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1983, 11, 22),
                        IsActive = true,
                        DepartmentID = 6
                    },
                    new Employee
                    {
                        EmployeeID = 23,
                        FirstName = "Fred",
                        LastName = "Durst",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1981, 2, 27),
                        IsActive = true,
                        DepartmentID = 7
                    },
                    new Employee
                    {
                        EmployeeID = 24,
                        FirstName = "Jonathan",
                        LastName = "Davis",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1978, 1, 20),
                        IsActive = true,
                        DepartmentID = 8
                    },
                    new Employee
                    {
                        EmployeeID = 25,
                        FirstName = "Pedro",
                        LastName = "Gil",
                        Gender = Gender.Male,
                        DateOfBirth = new DateTime(1970, 9, 20),
                        IsActive = false,
                        DepartmentID = 9
                    },
                    new Employee
                    {
                        EmployeeID = 26,
                        FirstName = "Sophia",
                        LastName = "Bo",
                        Gender = Gender.Female,
                        DateOfBirth = new DateTime(1987, 4, 15),
                        IsActive = true,
                        DepartmentID = 10
                    }
                );
        }
    }
}
