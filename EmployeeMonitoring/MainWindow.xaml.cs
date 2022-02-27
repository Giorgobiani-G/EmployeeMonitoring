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
        public MainWindow(EmpContext empContext)
        {
            InitializeComponent();
            context = empContext;
        }

        private void Gasvla_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Shesvla_Click(object sender, RoutedEventArgs e)
        {
            EmpModel empModel = new EmpModel();
            empModel.Saxeli = txtbox.Text;
          
        }
    }
}
