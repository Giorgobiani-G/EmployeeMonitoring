using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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



            var gasvla = (from db in context.MyProperty
                             where db.Saxeli == emp.Saxeli &&
                             db.ShesvlisDro.Value.Year == emp.ShesvlisDro.Value.Year &&
                             db.ShesvlisDro.Value.Month == emp.ShesvlisDro.Value.Month &&
                             db.WasvlisDro == null &&
                             db.GacceniliSaatebi != null
                          select db).Any();



            int dagvianeba = DateTime.Compare((DateTime)emp.ShesvlisDro, DateTime.Parse("22:14"));

            //tu daigviana

            if (dagvianeba > 0 && gasvla == false)
            {
                var variable = emp.ShesvlisDro.Value;
                TimeSpan timeSpan = (TimeSpan)(emp.ShesvlisDro - new DateTime(variable.Year, variable.Month, variable.Day, 9, 0, 0));
                emp.GacceniliSaatebi = timeSpan.TotalHours;
            }
            _ = await context.AddAsync(emp);
            _ = await context.SaveChangesAsync();
        }



        int count=0;
        private async void Gasvla_Click_1(object sender, RoutedEventArgs e)
        {
            
            count++;

            if (count > 1)
            {
                Gasvla.Click -= Gasvla_Click_1;
                System.Timers.Timer timer = new System.Timers.Timer(8000);
                timer.Enabled = false;
                timer.AutoReset = false;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                //timer.Close();
              
                count = 0;
                return;
            }

           

            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                WasvlisDro = DateTime.Now

            };

            

            var empfromdatabase = (from db in context.MyProperty
                                   where db.Saxeli == emp.Saxeli &&
                                   db.ShesvlisDro.Value.Year == emp.WasvlisDro.Value.Year &&
                                   db.ShesvlisDro.Value.Month == emp.WasvlisDro.Value.Month &&
                                    //db.GacceniliSaatebi != null&&
                                   db.WasvlisDro == null
                                   select db).ToArray().LastOrDefault();

            if (empfromdatabase != null)
            {
                var variabel = empfromdatabase.ShesvlisDro.Value;
                TimeSpan timeSpan = (TimeSpan)(emp.WasvlisDro - 
                    new DateTime(variabel.Year, variabel.Month, variabel.Day, variabel.Hour, variabel.Minute, variabel.Second));
                emp.GacceniliSaatebi = timeSpan.TotalHours;
            }
           
            _ = await context.AddAsync(emp);
            _ = await context.SaveChangesAsync();
           
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Gasvla.Click += Gasvla_Click_1;
        }
    }
}
