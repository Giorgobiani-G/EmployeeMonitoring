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
    /// Interaction logic for RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        private readonly EmpContext _context;
        public RegPage(EmpContext dbcontext)
        {
            _context = dbcontext;
            InitializeComponent();
            UserRegister.IsEnabled = false;
        }

        private void UserRegister_Click(object sender, RoutedEventArgs e)
        {
            var exists = (from db in _context.UserRegistrations
                          where db.UserName == UserName.Text && db.Password == Password.Password
                          select db).Any();
            if (exists)
            {
                MessageBox.Show("ასეთი მომხმარებელი უკვე არსებობს!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UserRegistration userRegistration = new UserRegistration();
                userRegistration.UserName = UserName.Text;
                userRegistration.Password = Password.Password;
                userRegistration.UserRole = RoleBox.SelectedItem.ToString();
                _context.UserRegistrations.Add(userRegistration);
                _context.SaveChanges();
                MessageBox.Show("მომხმარებელი წარმატებით დაემატა!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                

            }
        }



        private void UserName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(Password.Password)||UserName.Text.StartsWith(" "))
            {
                UserRegister.IsEnabled = false;
            }
            else
            {
                UserRegister.IsEnabled = true;
            }
        }



        private void Password_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(Password.Password)||Password.Password.StartsWith(" "))
            {
                UserRegister.IsEnabled = false;
            }
            else
            {
                UserRegister.IsEnabled = true;
            }
        }
    }
}
