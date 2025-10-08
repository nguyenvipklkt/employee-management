using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Function : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FunctionId { get; set; } // Số tự tăng
        public string FunctionCode { get; set; } = string.Empty; // Mã chức năng
        public string FunctionName { get; set; } = string.Empty; // Tên chức năng
    }
}
