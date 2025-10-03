using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Warehouse : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseId { get; set; } // Mã kho
        public string Name { get; set; } = string.Empty; // Tên kho
        public string Location { get; set; } = string.Empty; // Vị trí
        public int DepartmentId { get; set; } // Mã phòng ban
    }
}
