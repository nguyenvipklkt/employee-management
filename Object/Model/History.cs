using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class History : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryId { get; set; } // Mã lịch sử xuất/nhập hàng
        public int MaterialIdList { get; set; } // Nguyên liệu được xuất/nhập {1, 2, 3, ...}
        public float TotalPrice { get; set; } // Tổng tiền xuất/nhập
        public DateTime ImportDate { get; set; } // Ngày xuất/nhập hàng
        public string? Description { get; set; } // Ghi chú thêm
        public string HistoryType { get; set; } = string.Empty; // Loại nhập/xuất (I,E)
        public int DepartmentId { get; set; } // Mã cơ sở giao dịch
    }
}
