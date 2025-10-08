using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Material : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialId { get; set; } // Mã nguyên liệu
        public string Name { get; set; } = string.Empty; // Tên nguyên liệu
        public string Unit { get; set; } = string.Empty; // Đơn vị tính
        public long Price { get; set; } // Giá nhập
    }
}
