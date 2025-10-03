using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class TransactionType : BaseModel
    {
        public int TranTypeId { get; set; } // Mã loại giao dịch
        public string TranTypeName { get; set; } = string.Empty; // Tên loại giao dịch
        public string Unit { get; set; } = string.Empty; // Đơn vị
    }
}
