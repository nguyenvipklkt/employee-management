using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime Dob {  get; set; }
        public string? OTP { get; set; }
        public string? Address { get; set; }
    }
}
