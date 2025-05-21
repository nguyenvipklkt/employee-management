using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class Invoice : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; } // Mã hóa đơn
        public int UserId { get; set; } // Mã khách hàng (liên kết User)
        public DateTime CreatedAt { get; set; } // Ngày tạo hóa đơn
        public float TotalAmount { get; set; } // Tổng tiền
        public int TableId { get; set; } // Mã bàn
        public string QRCodeUrl { get; set; } = string.Empty; // Link mã QR của hóa đơn
    }
}
