using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace EmployeeMonitoring
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly EmpContext context;
        public RegistrationWindow(EmpContext dbcontext)
        {
            context = dbcontext;
            InitializeComponent();
            Closing += Window_Closing;


          


        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RegistrationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RegisterRegister_Click(object sender, RoutedEventArgs e)
        {
             

            var exits = (from db in context.EmpregisterModels
                         where db.EmployeeName == RegisterEmpName.Text
                         select db).Any();
            if (exits)
            {
                MessageBox.Show("ასეთი თანამშრომელი უკვე არსებობს!","თანამშრომელის დამატება:");

            }
            else
            {
                
                EmpregisterModel empregisterModel = new();
                empregisterModel.EmployeeName = RegisterEmpName.Text;
                empregisterModel.Salary = double.Parse(RegisterSalary.Text, System.Globalization.CultureInfo.InvariantCulture);
                context.Add(empregisterModel);
                context.SaveChanges();
                MessageBox.Show("თანამშრომელი წარმატებით დაემატა!", "თანამშრომელის დამატება:");
            }
                        


        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
          

            MainWindow mainWindow = new MainWindow(context);
            mainWindow.Show();
            Hide();

        }

       
    }
}
