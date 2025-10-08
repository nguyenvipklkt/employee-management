using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class ErrorDefinition : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorDefinitionId { get; set; } // Số tự tăng
        public string ErrorCode { get; set; } = string.Empty; // Mã lỗi
        public string ErrorName { get; set; } = string.Empty; // Tên lỗi
        public string? Description { get; set; } // mô tả
    }
}
