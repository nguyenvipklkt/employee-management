using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class User : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } // Mã người dùng
        public string Name { get; set; } = string.Empty; // Tên người dùng
        public string Email { get; set; } = string.Empty; // Email
        public string Password { get; set; } = string.Empty; // Mật khẩu người dùng
        public int IsActive { get; set; } // Tài khoản có hoat động hay không?
        public int RoleId { get; set; } // Mã quyền
        public DateTime Dob { get; set; } // Ngày sinh
        public int OTPId { get; set; } // OTPID
        public string Address { get; set; } = string.Empty; // Địa chỉ
        public int DepartmentId { get; set; } // Mã phòng ban
    }
}
