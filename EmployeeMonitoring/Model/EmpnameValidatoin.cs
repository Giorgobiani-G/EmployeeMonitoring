using System.Globalization;
using System.Windows.Controls;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace EmployeeMonitoring.Model
{
    public class EmpnameValidatoin : ValidationRule

    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            if (string.IsNullOrEmpty(value.ToString()))
            {
                result = new ValidationResult(false, "სახელი აუცილებელია!");
            }

            return result;
        }

    }

    }
