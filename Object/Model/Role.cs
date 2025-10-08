using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Role : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; } // Số tự tăng
        public string RoleCode { get; set; } = string.Empty; // Mã quyền
        public string RoleName { get; set; } = string.Empty; // Tên quyền
        public string FunctionCodeList { get; set; } = string.Empty; // Danh sách các mã chức năng
    }
}
