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
                    RoleName = RoleEnum.MANAGER,
                    FunctionCodeList = "Manager",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 3,
                    RoleName = RoleEnum.ACCOUNTANT,
                    FunctionCodeList = "Accountant",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 4,
                    RoleName = RoleEnum.WAREHOUSER,
                    FunctionCodeList = "Warehouser",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 5,
                    RoleName = RoleEnum.PANTRYMAN,
                    FunctionCodeList = "Pantryman",
                    CreateAt = DateTime.Now
                },
                new Role
                {
                    RoleId = 6,
                    RoleName = RoleEnum.STAFF,
                    FunctionCodeList = "Staff",
                    CreateAt = DateTime.Now
                }
            );
        }
    }
}
