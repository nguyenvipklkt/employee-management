using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Transaction : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; } // Mã giao dịch
        public int TranTypeId { get; set; } // Mã loại giao dịch
        public float Amount { get; set; } // Số lượng cần tăng/giảm
        public string TranType { get; set; } = string.Empty; // Tăng/Giảm  (C,D)
        public int RelatedEntityId { get; set; } // Mã item cần tăng/giảm
        public string CreatedBy { get; set; } = string.Empty; // Người thực hiện
    }
}
