using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Model
{
    [Table("Empregister", Schema = "dbo")]
    public class EmpregisterModel : INotifyPropertyChanged
    {
        private string _employeeName="emp";
        private double _salary;

        [Key]
        public int EmpregisterModelId { get; set; }


        public string EmployeeName
        {
            get
            {
                return _employeeName;
            }

            set
            {
                _employeeName = value;
                OnPropertyChanged("EmployeeName");
                
            }

        }



        public double Salary
        {
            get => _salary;

            set
            {
              
                _salary = value;
               OnPropertyChanged("Salary");
            }
        }



        public List<EmpModel> EmpModels { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }




    }

    }
