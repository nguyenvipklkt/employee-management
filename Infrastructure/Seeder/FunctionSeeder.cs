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
                     FunctionCode = "VIEW_PROFILE",
                     FunctionName = "Xem thông tin người dùng",
                     CreateAt = DateTime.Now
                 }
            );
        }
    }
}
