using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Seeds
{
    public static class AssignmentSeeder
    {
        public static void SeedAssignment(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>().HasData(
                new Assignment(1, 2, 12, 6),
                new Assignment(2, 2, 14, 6),
                new Assignment(3, 2, 16, 5),
                new Assignment(4, 2, 23, 12),
                new Assignment(5, 3, 1, 5),
                new Assignment(6, 3, 4, 11),
                new Assignment(7, 3, 7, 11),
                new Assignment(8, 3, 8, 5),
                new Assignment(9, 3, 9, 5),
                new Assignment(10, 3, 13, 11),
                new Assignment(11, 3, 14, 11),
                new Assignment(12, 3, 15, 3),
                new Assignment(13, 3, 21, 11),
                new Assignment(14, 3, 22, 12),
                new Assignment(15, 3, 24, 2),
                new Assignment(16, 3, 25, 11),
                new Assignment(17, 4, 1, 12),
                new Assignment(18, 4, 19, 4),
                new Assignment(19, 4, 20, 10),
                new Assignment(20, 5, 21, 10),
                new Assignment(21, 5, 22, 10),
                new Assignment(22, 6, 3, 7),
                new Assignment(23, 6, 4, 7),
                new Assignment(24, 6, 17, 9),
                new Assignment(25, 6, 19, 9),
                new Assignment(26, 7, 2, 8),
                new Assignment(27, 7, 12, 8),
                new Assignment(28, 7, 18, 8),
                new Assignment(29, 8, 1, 12),
                new Assignment(30, 9, 5, 1),
                new Assignment(31, 9, 6, 4),
                new Assignment(32, 9, 7, 12),
                new Assignment(33, 9, 11, 12),
                new Assignment(34, 9, 13, 10),
                new Assignment(35, 9, 16, 3),
                new Assignment(36, 9, 17, 2),
                new Assignment(37, 10, 8, 5),
                new Assignment(38, 10, 9, 5),
                new Assignment(39, 10, 10, 5),
                new Assignment(40, 10, 15, 5),
                new Assignment(41, 11, 1, 4),
                new Assignment(42, 11, 5, 1),
                new Assignment(43, 11, 18, 2));
        }
    }
}
