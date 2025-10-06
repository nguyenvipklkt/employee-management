using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Department : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; } // Mã Cơ sở
        public string DepartmentName { get; set; } = string.Empty; // Tên cơ sở
        public string DepartmentAddress { get; set; } = string.Empty; // Địa chỉ
        public int? ManagerId { get; set; } // Id quản lý
        public string? DepartmentPhoto { get; set; } = string.Empty; // Ảnh cơ sở
    }
}
