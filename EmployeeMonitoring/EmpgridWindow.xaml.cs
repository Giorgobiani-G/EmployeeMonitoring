﻿using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
