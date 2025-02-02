using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class UserFunction : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserFunctionId { get; set; }
        public string FunctionName { get; set; } = string.Empty;
    }
}
