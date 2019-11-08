using EFCore.DataLayer.EFClasses;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataLayer.Seeds
{
    public static class UserSeeder
    {
        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiClient>().HasData(
                new ApiClient(1, "mvc", "mvc", "client.mvc", "client.mvc"),
                new ApiClient(2, "razor", "razor", "client.razor", "client.razor"),
                new ApiClient(3, "xamarin", "xamarin", "client.xamarin", "client.xamarin"),
                new ApiClient(4, "android", "android", "client.android", "client.android")
            );
        }
    }
}
