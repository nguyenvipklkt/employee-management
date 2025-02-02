using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BaseModel
    {
        public DateTime CreateAt { get; set; } = DateTime.Now; 
        public DateTime UpdateAt { get; set; } = default; 
    }
}
