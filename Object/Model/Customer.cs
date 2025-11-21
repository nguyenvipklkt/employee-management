using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Customer : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; } // Mã khách hàng
        public string Name { get; set; } = string.Empty; // Tên khách hàng
        public string Email { get; set; } = string.Empty; // Email
        public DateTime? Dob { get; set; }
        public string? Address { get; set; }
    }
}
