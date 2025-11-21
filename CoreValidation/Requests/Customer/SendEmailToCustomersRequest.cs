using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreValidation.Requests.Customer
{
    public class SendEmailToCustomersRequest
    {
        public List<int> CustomerIdList { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
