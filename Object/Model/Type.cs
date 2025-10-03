using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Type : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; } // TypeID
        public string TypeKey { get; set; } = string.Empty; // Mã loại hình
        public string TypeName { get; set; } = string.Empty; // Tên loại hình
        public string TypeValue { get; set; } = string.Empty; // Giá trị loại hình
        public string Description { get; set; } = string.Empty; // Nội dung
    }
}
