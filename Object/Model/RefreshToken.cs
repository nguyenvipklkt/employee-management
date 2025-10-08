using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class RefreshToken : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RefreshTokenId { get; set; } // Mã refresh token
        public int UserId { get; set; } // Mã người dùng
        public string Token { get; set; } = string.Empty; // Chuỗi refresh token
        public DateTime ExpiresAt { get; set; } // Hạn dùng
        public string CreatedByIp { get; set; } = string.Empty; // IP đăng nhập
        public DateTime RevokedAt { get; set; } // Thu hồi chưa
        public string? ReplacedByToken { get; set; } // Nếu bị thay thì bị thay bởi token nào
        public int IsActive { get; set; } // Token còn hợp lệ không
    }
}
