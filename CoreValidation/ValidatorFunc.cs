using FluentValidation;

namespace CoreValidation
{
    public static class ValidatorFunc
    {
        public static void ValidateRequest<T>(IValidator<T> validator, T request)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorList = validationResult.Errors.FirstOrDefault();
                throw new Exception(errorList?.ToString());
            }
        }
    }
}
