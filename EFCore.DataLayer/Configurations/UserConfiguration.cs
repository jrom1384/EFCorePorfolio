using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Configurations
{
    public static class UserConfiguration
    {
        public static void ConfigureUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiClient>()
                .HasKey(u => u.ApiClientID);

            modelBuilder.Entity<ApiClient>()
                .HasIndex(i => i.Username)
                .IsUnique();
        }
    }
}
