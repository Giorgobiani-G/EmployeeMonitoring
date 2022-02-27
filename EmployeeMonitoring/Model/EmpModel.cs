using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Model
{
    [Table("EmpMonitor",Schema ="dbo")]
   public class EmpModel
    {

        [Key]
        public int Id { get; set; }
        public string Saxeli { get; set; }
        public DateTime? ShesvlisDro { get; set; }

        public DateTime? WasvlisDro { get; set; }

        public double? GacceniliSaatebi { get; set; }


       

    }
}
