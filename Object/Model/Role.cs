using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Role : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; } // Mã quyền
        public string RoleName { get; set; } = string.Empty; // Tên quyền
        public string FunctionCodeList { get; set; } = string.Empty; // Danh sách các mã chức năng
        public DateTime StartTime { get; set; } // Thời gian bắt đầu sử dụng chức năng
        public DateTime EndTime { get; set; } // Thời gian kết thúc sử dụng chức năng
        public int ParentId { get; set; } // Mã quyền cha (nếu để trống thì là quyền cha)
    }
}
