using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EmployeeMonitoring.Model
{
    public class SalaryValidatoin : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            bool isnullorempty = string.IsNullOrEmpty(value.ToString());
            

            if (isnullorempty)
            {
                result = new ValidationResult(false, "არასწორი ხელფასი!");
            }
            
            return result;
        }
    }
}
