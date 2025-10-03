using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class ErrorDefinition : BaseModel
    {
        public int AutoId { get; set; } // Số tự tăng
        public string ErrorId { get; set; } = string.Empty; // Mã lỗi
        public string ErrorName { get; set; } = string.Empty; // Tên lỗi
        public string Description { get; set; } = string.Empty; // mô tả
    }
}
