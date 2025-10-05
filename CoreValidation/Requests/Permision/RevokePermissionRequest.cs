namespace CoreValidation.Requests.Permision
{
    public class RevokePermissionRequest
    {
        public int TargetUserId { get; set; }
        public int FunctionId { get; set; }
    }
}
