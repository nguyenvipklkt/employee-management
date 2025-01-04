using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Dob {  get; set; }
        public string OTP { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
