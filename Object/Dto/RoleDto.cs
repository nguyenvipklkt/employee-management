namespace Object.Dto
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string FunctionCodesList { get; set; } = string.Empty;
        public int IsSuperAdmin { get; set; }
    }
}