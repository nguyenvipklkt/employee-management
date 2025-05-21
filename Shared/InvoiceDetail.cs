using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class InvoiceDetail : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceDetailId { get; set; } // Mã chi tiết hóa đơn
        public int InvoiceId { get; set; } // Mã hóa đơn
        public int FoodId { get; set; } // Mã món ăn
        public int Quantity { get; set; } // Số lượng
        public float UnitPrice { get; set; } // Đơn giá lúc mua
    }
}
