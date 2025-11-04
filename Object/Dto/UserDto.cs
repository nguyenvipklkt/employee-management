namespace Object.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public int IsSuperAdmin { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string Address { get; set; } = string.Empty;
        public string StatusAccount { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
