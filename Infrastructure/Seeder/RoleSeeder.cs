using Microsoft.EntityFrameworkCore;
using Object.Model;

namespace Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static void SeedRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    RoleName = "Admin",
                    UserFunctionIdList = string.Empty,
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "Employee",
                    UserFunctionIdList = string.Empty,
                    CreateAt = DateTime.Now
                }
            );
        }
    }
}
