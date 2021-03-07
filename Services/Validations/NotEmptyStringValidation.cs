using System.Globalization;
using System.Windows.Controls;

namespace TradeStats.Services.Validations
{
    class NotEmptyStringValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "Field can't be empty");

            return ValidationResult.ValidResult;
        }
    }
}
