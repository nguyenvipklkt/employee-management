using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class OTP : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OTPId { get; set; } // OTPID
        public int UserId { get; set; } // Mã người dùng
        public string Description { get; set; } = string.Empty; // Nội dung OTP
        public string OTPType { get; set; } = string.Empty; // Loại OTP
        public int Status { get; set; } // Trang thái của OTP
        public DateTime ExpiryTime { get; set; } // Thời gian hết hạn của OTP
    }
}
