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
using System.Windows.Shapes;

namespace EmployeeMonitoring
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {

        private readonly EmpContext _context;



        public Users(EmpContext dbcontext)
        {
            _context = dbcontext;
            InitializeComponent();
            Main.Content = new LoginPage(_context);
        }

         

        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new LoginPage(_context);
        }

        private void TabItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
           
            Main.Content = new RegPage(_context);
        }
    } 
}
