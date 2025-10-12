namespace CoreValidation.Requests.Permision
{
    public class RevokePermissionRequest
    {
        public int TargetUserId { get; set; }
        public string FunctionCode { get; set; } = string.Empty;
    }
}
