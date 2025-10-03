using System;

namespace Object.Dto
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string UserFunctionIdList { get; set; } = string.Empty;
    }
}