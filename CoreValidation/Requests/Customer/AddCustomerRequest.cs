using CoreValidation.Requests.Authentication;
using CoreValidationRules;
using FluentValidation;

namespace CoreValidation.Requests.Customer
{
    public class AddCustomerRequest
    {
        public string Name { get; set; } = string.Empty; // Tên khách hàng
        public string Email { get; set; } = string.Empty; // Email
        public string? Dob { get; set; } // Ngày sinh
        public string? Address { get; set; } = string.Empty; // Địa chỉ
    }

    public class AddCustomerRequestValidation : BaseRule<AddCustomerRequest>
    {
        public AddCustomerRequestValidation()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tên khách hàng không được bỏ trống.")
                .MaximumLength(250).WithMessage("Tên khách hàng chỉ chứa tối đa 250 ký tự.");
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email không được bỏ trống.")
                .Must(IsValidEmail).WithMessage("Email không đúng định dạng.")
                .MaximumLength(250).WithMessage("Email chỉ chứa tối đa 250 ký tự.");
        }
    }
}
