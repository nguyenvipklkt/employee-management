namespace Object.Dto
{
    public class DepartmentDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DepartmentAddress { get; set; } = string.Empty;
        public string DepartmentPhoto { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
    }
}
