using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Department : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; } // Mã phòng ban
        public string DepartmentName { get; set; } = string.Empty; // Tên phòng ban
        public string DepartmentAddress { get; set; } = string.Empty; // Địa chỉ
        public int DepartmentTypeId { get; set; } // Loại phòng ban
    }
}
