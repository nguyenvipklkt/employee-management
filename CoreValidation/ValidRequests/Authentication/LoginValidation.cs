using CoreValidation.Requests.Authentication;
using CoreValidationRules;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CoreValidation.ValidRequests.Authentication
{
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
