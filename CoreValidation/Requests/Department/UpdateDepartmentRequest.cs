using Microsoft.AspNetCore.Http;

namespace CoreValidation.Requests.Department
{
    public class UpdateDepartmentRequest
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentAddress { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
