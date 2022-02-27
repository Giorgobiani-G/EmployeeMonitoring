using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EmpContext context;


        public MainWindow()
        {
            InitializeComponent();
            context = new EmpContext();
        }

       

        private async void Shesvla_Click(object sender, RoutedEventArgs e)
        {
            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                ShesvlisDro = DateTime.Now
                 
            };

            var gasvlisdro = from db in context.MyProperty
                             where db.Saxeli == emp.Saxeli
                             select new{ db.WasvlisDro };
              
            
          int dagvianeba =  DateTime.Compare((DateTime)emp.ShesvlisDro, DateTime.Parse("09:14"));

            if (dagvianeba > 0 && gasvlisdro is null)
            {
              emp.GacceniliSaatebi =  emp.ShesvlisDro.Value.Subtract(DateTime.Parse("09:00")).TotalHours;
            }
            _ = await context.AddAsync(emp);
            _ = await context.SaveChangesAsync();
        }

        private async void Gasvla_Click_1(object sender, RoutedEventArgs e)
        {
            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                WasvlisDro = DateTime.Now
            };
            _ = await context.AddAsync(emp);
            _ = await context.SaveChangesAsync();
        }
    }
}
