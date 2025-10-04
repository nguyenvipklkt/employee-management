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
                    FunctionIdList = "1",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "Employee",
                    FunctionIdList = "1",
                    CreateAt = DateTime.Now
                }
            );
        }
    }
}
