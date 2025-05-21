using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreValidation.Requests.Authentication
{
    public class RefreshRequestToken
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
