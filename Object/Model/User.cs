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
        public string RoleCode { get; set; } = string.Empty; // Mã quyền
        public DateTime Dob { get; set; } // Ngày sinh
        public int OTPId { get; set; } // OTPID
        public string Address { get; set; } = string.Empty; // Địa chỉ
        public int DepartmentId { get; set; } // Mã cơ sở
        public int IsSuperAdmin { get; set; } // Có quyền tuyệt đối hay không
        public DateTime? LastLoginAt { get; set; } // Đăng nhập vào
        public int CanGrantPermission { get; set; } // Có thể gán quyền hay không
        public string? StartTime { get; set; } // Thời gian bắt đầu sử dụng chức năng
        public string? EndTime { get; set; } // Thời gian kết thúc sử dụng chức năng
    }
}
