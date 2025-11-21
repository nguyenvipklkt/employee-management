using Common.Enum.RoleEnum;
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
                    RoleCode = RoleEnum.ADMIN,
                    RoleName = "Admin",
                    FunctionCodeList = "",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = RoleEnum.CUSTOMER,
                    FunctionCodeList = "Customer",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 6,
                    RoleName = RoleEnum.EMPLOYEE,
                    FunctionCodeList = "Employee",
                    CreateAt = DateTime.Now
                }
            );
        }
    }
}
