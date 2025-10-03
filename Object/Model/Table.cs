using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Table : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableId { get; set; } // Mã bàn
        public string TableNumber { get; set; } = string.Empty; // Số hiệu bàn
        public string QRCodeUrl { get; set; } = string.Empty; // Link đến QR
        public string Status { get; set; } = string.Empty; // Trạng thái (trống, đang phục vụ...)
        public int Capacity { get; set; } // Sức chứa
    }
}
