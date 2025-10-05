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
        public int GrantorId { get; set; } // Ai là người có quyền cấp quyền (UserId hoặc RoleId)
        public int FunctionId { get; set; } // Chức năng được cấp
        public int CanGrant { get; set; } // Có thể gán quyền này cho người khác không?
        public string Scope { get; set; } = string.Empty; // Optional
        public string GrantorType { get; set; } = string.Empty; // loại grantor
    }
}
