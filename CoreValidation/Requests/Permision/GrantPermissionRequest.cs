namespace CoreValidation.Requests.Authentication
{
    public class GrantPermissionRequest
    {
        public int TargetUserId { get; set; }
        public List<string> FunctionCodes { get; set; } = new List<string>();
    }
}
