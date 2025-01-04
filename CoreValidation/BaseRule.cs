using FluentValidation;
using Helper.NLog;

namespace CoreValidationRules
{
    public abstract class BaseRule<T> : AbstractValidator<T>
    {
        protected bool IsValidNumberPrecision<T>(T value, int maxDigits) where T : struct, IConvertible
        {
            try
            {
                var decimalValue = Convert.ToDecimal(value);
                if (decimalValue.ToString().Contains(".")) 
                {
                    var parts = decimalValue.ToString("G29").Split('.');

                    var integerPartLength = parts[0].Length;
                    var decimalPartLength = parts.Length > 1 ? parts[1].Length : 0;

                    return integerPartLength + decimalPartLength <= maxDigits;
                }
                else
                {
                    var parts = decimalValue.ToString("G29").Split(',');
                    var integerPartLength = parts[0].Length;
                    var decimalPartLength = parts.Length > 1 ? parts[1].Length : 0;

                    return integerPartLength + decimalPartLength <= maxDigits;
                }
                
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                return true;
            }
        }

        protected bool IsValidIntPartNumPrecision<T>(T value, int maxDigits) where T : struct, IConvertible
        {
            try
            {
                var decimalValue = Convert.ToDecimal(value);
                if (decimalValue.ToString().Contains("."))
                {
                    var parts = decimalValue.ToString("G29").Split('.');

                    var integerPartLength = parts[0].Length;

                    return integerPartLength <= maxDigits;
                }
                else
                {
                    var parts = decimalValue.ToString("G29").Split(',');
                    var integerPartLength = parts[0].Length;

                    return integerPartLength <= maxDigits;
                }

            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                return true;
            }
        }

        protected bool IsValidDecPartNumPrecision<T>(T value, int maxDigits) where T : struct, IConvertible
        {
            try
            {
                var decimalValue = Convert.ToDecimal(value);
                if (decimalValue.ToString().Contains("."))
                {
                    var parts = decimalValue.ToString("G29").Split('.');

                    var decimalPartLength = parts.Length > 1 ? parts[1].Length : 0;

                    return decimalPartLength <= maxDigits;
                }
                else
                {
                    var parts = decimalValue.ToString("G29").Split(',');
                    var decimalPartLength = parts.Length > 1 ? parts[1].Length : 0;

                    return decimalPartLength <= maxDigits;
                }

            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                return true;
            }
        }
        protected bool IsGreaterThan<T>(T value, T minValue) where T : struct, IComparable<T>
        {
            try
            {
                // So sánh giá trị
                return value.CompareTo(minValue) >= 0;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                return false; // Trả về false nếu xảy ra lỗi
            }
        }

        protected bool IsValidEmail(string? email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;
                var patternEmail = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, patternEmail);
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                return false;
            }
        }

        protected bool IsValidSpaceOrSpecialCharOrAccentedChar(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var pattern = @"^[a-zA-Z0-9]*$";
            return System.Text.RegularExpressions.Regex.IsMatch(value, pattern);
        }

    }
}
