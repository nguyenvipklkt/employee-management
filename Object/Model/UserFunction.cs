using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class UserFunction : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserFunctionId { get; set; } // Mã chức năng của người dùng
        public int FunctionId { get; set; } // Mã chức năng
        public int UserId { get; set; } // Mã người dùng
        public int GrantorId { get; set; } // Người gán quyền
        public DateTime GrantAt { get; set; } // Thời điểm gán
    }
}
