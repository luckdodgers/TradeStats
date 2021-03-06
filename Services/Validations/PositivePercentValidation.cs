using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TradeStats.Services.Validations
{
    class PositivePercentValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString() == string.Empty)
                return new ValidationResult(false, "Value can't be empty.");

            var isDecimal = decimal.TryParse(value.ToString(), out var number);

            return (isDecimal && number >= 0 && number <= 100) ?
                ValidationResult.ValidResult : new ValidationResult(false, "Needs positive numeric value less or equal than 100");
        }
    }
}
