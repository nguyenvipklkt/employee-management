namespace CoreValidation.Requests.Authentication
{
    public class VerifyCode
    {
        public string? Email { get; set; }
        public string? VerificationCode { get; set; }
    }
}
