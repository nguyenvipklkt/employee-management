using Microsoft.EntityFrameworkCore;
using Object.Model;

namespace Infrastructure.Seeder
{
    public static class UserFunctionSeeder
    {
        public static void SeedUserFunction(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFunction>().HasData(
                 new UserFunction
                 {
                     UserFunctionId = 1,
                     FunctionName = "VIEW_USER",
                     CreateAt = DateTime.Now
                 }
            );
        }
    }
}
