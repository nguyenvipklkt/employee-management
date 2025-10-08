using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class FunctionGrantPermission : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FunctionGrantPermissionId { get; set; } // Số tự tăng
        public int GrantorId { get; set; } // Ai là người có quyền cấp quyền
        public int FunctionCode { get; set; } // Chức năng được cấp
    }
}
