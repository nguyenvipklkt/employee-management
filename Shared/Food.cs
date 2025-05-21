using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class Food : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodId { get; set; } // Mã món ăn
        public string Name { get; set; } = string.Empty; // Tên món ăn
        public string Description { get; set; } = string.Empty; // Mô tả món ăn
        public float Price { get; set; } // Giá món ăn
        public string Unit { get; set; } = string.Empty; // Đơn vị tính
        public string ImageUrl { get; set; } = string.Empty; // Ảnh món ăn
    }
}
