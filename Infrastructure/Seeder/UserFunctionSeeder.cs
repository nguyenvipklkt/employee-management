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
                     FunctionId = 1,
                     UserId = 1,
                     CreateAt = DateTime.Now,
                 }
            );
        }
    }
}
