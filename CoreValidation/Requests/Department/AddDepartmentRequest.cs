namespace CoreValidation.Requests.Department
{
    public class AddDepartmentRequest
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentAddress { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public Stream? Photo { get; set; }
    }
}
