using EmployeeMonitoring.Data;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly EmpContext _context;
        public LoginPage(EmpContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var users = (from db in _context.UserRegistrations
                        where db.UserName==LoginName.Text&&db.Password==LoginPass.Password
                         select db).Any();

            if (users)
            {
                GlobalCustom.CurrentUserName = LoginName.Text;

                MainWindow mainWindow = new MainWindow(_context);
                 
                mainWindow.Show();

                Window.GetWindow(this).Hide();

                

            }

            else
            {
                MessageBox.Show("ასეთი იუზერი არ არსებობს!", "", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
    }
}
