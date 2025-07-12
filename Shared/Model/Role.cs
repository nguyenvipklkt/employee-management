using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Object.Model
{
    public class Role : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string UserFunctionIdList { get; set; } = string.Empty;
    }
}
