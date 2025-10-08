using Microsoft.AspNetCore.Http;

namespace CoreValidation.Requests.Department
{
    public class AddDepartmentRequest
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentAddress { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
