using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class WarehouseStock : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; } // Mã liên kết kho
        public int WarehouseId { get; set; } // Mã kho
        public int MaterialId { get; set; } // Mã nguyên liệu
        public float Quantity { get; set; } // Số lượng còn lại của lô
        public float UnitPrice { get; set; } // Giá nhập
        public DateTime ImportedAt { get; set; } // Ngày nhập lô
        public DateTime ExpiredAt { get; set; } // Hạn sử dụng (nếu có)
        public string Note { get; set; } = string.Empty; // Ghi chú lô hàng
        public bool IsUsedUp { get; set; } // Đã dùng hết chưa
    }
}
