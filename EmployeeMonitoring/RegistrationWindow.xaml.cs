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


            RegisterRegister.IsEnabled = false;


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
                MessageBox.Show("ასეთი თანამშრომელი უკვე არსებობს!", "თანამშრომელის დამატება:");

            }
            else
            {

                EmpregisterModel empregisterModel = new();
                empregisterModel.EmployeeName = RegisterEmpName.Text;
                empregisterModel.Salary = decimal.Parse(RegisterSalary.Text, System.Globalization.CultureInfo.InvariantCulture);
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

        private void RegisterEmpName_LostFocus(object sender, RoutedEventArgs e)
        {
            string txtboxvalue;
            bool istrue = RegisterSalary.Text.Contains(".");
            if (istrue)
            {
                txtboxvalue = RegisterSalary.Text.Replace(".", ",");

            }
            else
            {
                txtboxvalue = RegisterSalary.Text;
            }
            try
            {
                double convert = Convert.ToDouble(txtboxvalue);

                if (string.IsNullOrEmpty(RegisterEmpName.Text) || string.IsNullOrEmpty(RegisterSalary.Text))
                {
                    RegisterRegister.IsEnabled = false;
                }
                else
                {
                    RegisterRegister.IsEnabled = true;
                }
            }
            catch (Exception)
            {

                RegisterRegister.IsEnabled = false;
            }
        }

        private void RegisterSalary_LostFocus(object sender, RoutedEventArgs e)
        {
            

            string txtboxvalue;
            double convert;
            bool istrue = RegisterSalary.Text.Contains(".");
            if (istrue)
            {
                 txtboxvalue = RegisterSalary.Text.Replace(".", ",");
               
            }
            else
            {
                txtboxvalue = RegisterSalary.Text;
            }

            try
            {
                if (string.IsNullOrEmpty(txtboxvalue)||string.IsNullOrEmpty(RegisterEmpName.Text))
                {
                    RegisterRegister.IsEnabled = false;
                }
                else
                {
                    convert = Convert.ToDouble(txtboxvalue);
                    RegisterRegister.IsEnabled = true;
                }
                
            }
            catch (Exception)
            {

                RegisterRegister.IsEnabled = false;
            }


            
        }
    }
}
