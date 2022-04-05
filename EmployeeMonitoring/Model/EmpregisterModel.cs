using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Model
{
    [Table("Empregister", Schema = "dbo")]
    public class EmpregisterModel
    {
        [Key]
        public int EmpregisterModelId { get; set; }
        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public double Salary { get; set; }


        public List<EmpModel> EmpModels { get; set; }

    }
}
