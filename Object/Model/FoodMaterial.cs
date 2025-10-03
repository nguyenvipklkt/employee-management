using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class FoodMaterial : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodMaterialId { get; set; } // Mã dòng liên kết
        public int FoodId { get; set; } // Mã món ăn
        public int MaterialId { get; set; } // Mã nguyên liệu
        public float Quantity { get; set; } // Số lượng nguyên liệu cần
    }
}
