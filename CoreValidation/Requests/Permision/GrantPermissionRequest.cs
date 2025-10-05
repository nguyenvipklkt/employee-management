namespace CoreValidation.Requests.Authentication
{
    public class GrantPermissionRequest
    {
        public int TargetUserId { get; set; }
        public List<int> FunctionIds { get; set; } = new List<int>();
    }
}
