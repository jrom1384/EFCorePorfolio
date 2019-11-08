using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Seeds
{
    public static class ProjectSeeder
    {
        public static void SeedProject(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    ProjectID = 1,
                    ProjectName = "Home Automation - Raspberry Pi Aquarium Pump Scheduler",
                    MemberCount = 0
                },
                new Project
                {
                    ProjectID = 2,
                    ProjectName = "Home Automation - Android/Arduino NFC Relay Controller",
                    MemberCount = 4
                },
                new Project
                {
                    ProjectID = 3,
                    ProjectName = "Empire B - Piso Wifi Build & Deploy",
                    MemberCount = 12
                },
                new Project
                {
                    ProjectID = 4,
                    ProjectName = "ASP.Net Core MVC Project",
                    MemberCount = 3
                },
                new Project
                {
                    ProjectID = 5,
                    ProjectName = "ASP.Net Core Razor Page Project",
                    MemberCount = 2
                },
                new Project
                {
                    ProjectID = 6,
                    ProjectName = "ASP.Net Core Web API Project with JWT & Swagger",
                    MemberCount = 4
                },
                new Project
                {
                    ProjectID = 7,
                    ProjectName = "N-Tier Web Based Application",
                    MemberCount = 3
                },
                new Project
                {
                    ProjectID = 8,
                    ProjectName = "Raspberry Pi - GSM SMS Controller",
                    MemberCount = 1
                },
                new Project
                {
                    ProjectID = 9,
                    ProjectName = "Android Mobile OTP Decoder Application",
                    MemberCount = 7
                },
                new Project
                {
                    ProjectID = 10,
                    ProjectName = "Interview Rehersal Training",
                    MemberCount = 4
                },
                new Project
                {
                    ProjectID = 11,
                    ProjectName = "Agile Project Management with Scrum",
                    MemberCount = 3
                }
            );
        }
    }
}
