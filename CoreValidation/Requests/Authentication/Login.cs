using CoreValidationRules;
using FluentValidation;

namespace CoreValidation.Requests.Authentication
{
    public class Login
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginValidation : BaseRule<Login>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email không được bỏ trống.")
                .Must(IsValidEmail).WithMessage("Email không đúng định dạng.")
                .MaximumLength(250).WithMessage("Email chỉ chứa tối đa 250 ký tự.");
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Mật khẩu không được bỏ trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải chứa tối thiểu 6 ký tự");
        }
    }
}
