using BCrypt;
using BCrypt.Net;

namespace Helper.BCryptHelper
{
    public static class BCryptHelper
    {
        // Mã hóa mật khẩu
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Xác thực mật khẩu
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
