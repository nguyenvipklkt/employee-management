using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum
{
    public class APIResponse
    {
        public string Code { get; set; } = "Ok";
        public string Message { get; set; } = "";
        public object? Data { get; set; }
    }
}
