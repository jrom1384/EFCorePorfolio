using EFCore.DataLayer.Configurations;
using EFCore.DataLayer.EFClasses;
using EFCore.DataLayer.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option)
            : base(option)
        {

        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Project> Project { get; set; }

        public DbSet<Assignment> Assignment { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet <ApiClient> ApiClient { get; set; }

        //Add-Migration MigrationName
        //Update-Database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Table Configurations
            modelBuilder.ConfigureDepartment();
            modelBuilder.ConfigureProject();
            modelBuilder.ConfigureRole();
            modelBuilder.ConfigureUser();
            modelBuilder.ConfigureEmployee();
            modelBuilder.ConfigureAssignment();

            //Seed Data to table
            modelBuilder.SeedDepartment();
            modelBuilder.SeedProject();
            modelBuilder.SeedRole();
            modelBuilder.SeedUser();
            modelBuilder.SeedEmployee();
            modelBuilder.SeedAssignment();
        }

    }
}
