using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreValidation.Requests.Authentication
{
    public class VerifyCode
    {
        public string? Email { get; set; }
        public string? VerificationCode { get; set; }
    }
}
