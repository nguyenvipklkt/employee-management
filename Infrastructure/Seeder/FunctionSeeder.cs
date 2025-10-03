using Microsoft.EntityFrameworkCore;
using Object.Model;

namespace Infrastructure.Seeder
{
    public static class FunctionSeeder
    {
        public static void SeedFunction(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Function>().HasData(
                 new Function
                 {
                     FunctionId = 1,
                     FunctionName = "VIEW_USER",
                     CreateAt = DateTime.Now
                 }
            );
        }
    }
}
