using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            
           

            RegisterRegister.IsEnabled = false;
            //Main.Content = new RegPage(context);

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
                MessageBox.Show("ასეთი თანამშრომელი უკვე არსებობს!","",MessageBoxButton.OK,MessageBoxImage.Error);

            }
            else
            {
                EmpregisterModel empregisterModel = new();
                empregisterModel.EmployeeName = RegisterEmpName.Text;
                empregisterModel.Salary = decimal.Parse(RegisterSalary.Text, System.Globalization.CultureInfo.InvariantCulture);
                empregisterModel.Isactive = true;
                context.Add(empregisterModel);
                context.SaveChanges();
                MessageBox.Show("თანამშრომელი წარმატებით დაემატა!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(context);
            mainWindow.Show();
            Regwindow.Hide();
        }

         

        private void RegisterEmpName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(RegisterSalary.Text) || string.IsNullOrWhiteSpace(RegisterEmpName.Text) || RegisterEmpName.Text.StartsWith(" "))
                {
                    RegisterRegister.IsEnabled = false;
                }
                else
                {
                    RegisterRegister.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void RegisterSalary_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
           {
                if (string.IsNullOrEmpty(RegisterEmpName.Text) ||
                    Regex.IsMatch(RegisterSalary.Text, @"^(0|[1-9]\d*)(\.\d{1,2})?$") == false) // მხოლოდ იღებს  int-ს და decimal-ს წერტილის შემდეგ 2 თანრიგით.
                {
                    RegisterRegister.IsEnabled = false;
                }
                else
                {

                    RegisterRegister.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
