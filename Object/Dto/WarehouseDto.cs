using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object.Dto
{
    public class WarehouseDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
