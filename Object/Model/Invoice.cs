using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Invoice : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; } // Mã hóa đơn
        public string PhoneNumber { get; set; } = string.Empty; // Số điện thoại khách hàng
        public string CusName { get; set; } = string.Empty; // Tên khách hàng
        public float TotalAmount { get; set; } // Tổng tiền
        public int TableId { get; set; } // Mã bàn
        public string QRCodeUrl { get; set; } = string.Empty; // Link mã QR của hóa đơn
    }
}
