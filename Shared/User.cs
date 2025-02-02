using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class User : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IsActive { get; set; } = 0;
        public int RoleId { get; set; }
        public DateTime Dob {  get; set; }
        public string OTP { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }
}
