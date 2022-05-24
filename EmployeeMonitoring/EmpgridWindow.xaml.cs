using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace EmployeeMonitoring
{
    /// <summary>
    /// Interaction logic for EmpgridWindow.xaml
    /// </summary>
    public partial class EmpgridWindow : Window
    {
        private EmpContext _empcontext;
        public EmpgridWindow(EmpContext empcontext)
        {
            _empcontext = empcontext;
            InitializeComponent();
            
        }

        private void GridData_Loaded(object sender, RoutedEventArgs e)
        {
            var data = (from db in _empcontext.EmpregisterModels
                        select db).ToList(); /*new {Id = db.EmpregisterModelId, Name = db.EmployeeName, Salry= db.Salary }).ToList();*/

            GridData.ItemsSource = data;
        }

        private void GridData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmpregisterModel model = (EmpregisterModel)GridData.SelectedItem;
            if (model != null)
            {
                gridboxid.Text = model.EmpregisterModelId.ToString();
                gridboxname.Text = model.EmployeeName;
                gridboxsalary.Text = model.Salary.ToString();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(gridboxsalary.Text)!=true)
            {
                 var ee = (from db in _empcontext.EmpregisterModels
                         where db.EmpregisterModelId.ToString() == gridboxid.Text
                         select db).FirstOrDefault();

                ee.Salary = Convert.ToDecimal(gridboxsalary.Text);

                _empcontext.EmpregisterModels.Update(ee);
                _empcontext.SaveChanges();

                System.Timers.Timer timer1 = new System.Timers.Timer(3500);
                timer1.Enabled = true;
                timer1.AutoReset = false;
                timer1.Elapsed += Timer1_Elapsed;
                timer1.Start();

                editresult.Text = "მონაცემები წარმატებით შეიცვლა!";
                 
            }
            
        }

        
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                editresult.Clear();
            });
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            gridwindow.Close();
        }
    }
}
