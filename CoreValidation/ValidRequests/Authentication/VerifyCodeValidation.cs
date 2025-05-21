using CoreValidation.Requests.Authentication;
using CoreValidationRules;
using FluentValidation;

namespace CoreValidation.ValidRequests.Authentication
{
    public class VerifyCodeValidation : BaseRule<VerifyCode>
    {
        public VerifyCodeValidation()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email không được bỏ trống.")
                .Must(IsValidEmail).WithMessage("Email không đúng định dạng.")
                .MaximumLength(250).WithMessage("Email chỉ chứa tối đa 250 ký tự.");
            RuleFor(x => x.VerificationCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Mã xác thực không được bỏ trống")
                .Length(6).WithMessage("Mã xác thực phải chứa 6 ký tự");
        }
    }
}
