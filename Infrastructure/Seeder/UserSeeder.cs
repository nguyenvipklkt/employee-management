using Helper.BCryptHelper;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "Nguyên Trung",
                    Email = "nguyenhua2508@gmail.com",
                    Password = BCryptHelper.HashPassword("nguyen250802"),
                    Dob = new DateTime(2002, 08, 25),
                    OTP = string.Empty,
                    Address = "Hà Nội",
                    CreateAt = DateTime.Now,
                    RoleId = 1,
                    IsActive = 1
                }
            );
        }
    }
}
