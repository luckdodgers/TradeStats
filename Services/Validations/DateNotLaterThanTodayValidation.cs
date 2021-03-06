using System;
using System.Globalization;
using System.Windows.Controls;

namespace TradeStats.Services.Validations
{
    class DateNotLaterThanTodayValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isDate = DateTime.TryParse(value.ToString(), out var datetime);

            if (isDate && datetime.Date <= DateTime.Today)
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Date can't be later than today");
        }
    }
}
