using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class OrderTable : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderTableId { get; set; } // Mã đặt bàn
        public string TableId { get; set; } = string.Empty; // Mã bàn
        public string PhoneNumber { get; set; } = string.Empty; // Số điện thoại khách hàng
        public string Address { get; set; } = string.Empty; // Tên khách hàng
        public int DepartmentId { get; set; } // Mã cơ sở
    }
}
