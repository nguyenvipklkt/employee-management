using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreValidation.Requests.Department
{
    public class UpdateDepartmentRequest
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentAddress { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public Stream? Photo { get; set; }
    }
}
