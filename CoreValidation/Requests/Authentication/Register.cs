using CoreValidationRules;
using FluentValidation;

namespace CoreValidation.Requests.Authentication
{
    public class Register
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterValidation : BaseRule<Register>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tên không được bỏ trống.")
                .MaximumLength(20).WithMessage("Email chỉ chứa tối đa 20 ký tự.");
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
