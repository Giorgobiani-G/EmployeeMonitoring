using ClosedXML.Excel;
using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace EmployeeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EmpContext context;
        List<string> list = new List<string>();
        List<string> list12 = new List<string>();
        List<string> list2 = new List<string>() { "5", "10", "15", "20", "25", "30" };


        public MainWindow(EmpContext dbcontext)
        {
            context = dbcontext;
            InitializeComponent();

            for (int i = 0; i <= 24; i++)
            {
                list.Add(i.ToString() + ":" + "00");
                if (i != 24)
                {
                    list.Add(i.ToString() + ":" + "30");
                }


            }

           int index = list.FindIndex(a => a.Contains(stcombo.SelectedValuePath.ToString()));
            for (int i = index; i < 24; i++)
            {
                list12.Add(i.ToString() + ":" + "00");
                if (i != 24)
                {
                    list12.Add(i.ToString() + ":" + "30");
                }

            }

            stcombo.ItemsSource = list;
            endcombo.ItemsSource = list12;
            mrgcombo.ItemsSource = list2;
            stcombo.SelectedValue = Properties.Settings.Default.StartTime;
            endcombo.SelectedValue = Properties.Settings.Default.EndTIme;
            mrgcombo.SelectedValue = Properties.Settings.Default.Margin;
            Properties.Settings.Default.Save();

            DateTime time = DateTime.Now;
            DateTime target = new DateTime(time.Year, time.Month, time.Day, 22, 55, 0);
            double interval = (target - DateTime.Now).TotalMilliseconds;
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += Daangarisheba;
            timer.Enabled = false;
            timer.AutoReset = false;
            timer.Start();
        }



        private readonly object outputLock = new();

        private void Daangarisheba(object sender, System.Timers.ElapsedEventArgs e)
        {
             
            Dispatcher.Invoke(() =>
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                   

                    var query = (from db in context.MyProperty.Where(i => i.EmpregisterModel.Isactive == true).AsEnumerable()
                                 where (db.ShesvlisDro.HasValue && db.ShesvlisDro.Value.Date == DateTime.Now.Date && db.GacceniliSaatebi == null)
                                 || (db.WasvlisDro.HasValue && db.WasvlisDro.Value.Date == DateTime.Now.Date && db.GacceniliSaatebi == null)

                                 group db by db.Saxeli).ToList();

                    #region თუ დასვენების დღე არაა და  რომელიმე თანამშრომელი არ გამოცხედებულა გაცდენილი საათების რაოდენობა უდრის 8 საათს
                    if (query is not null)
                    {

                        HashSet<string> keys = new HashSet<string>(query.Select(k => k.Key));
                        var contains = (context.EmpregisterModels.Where(m => !keys.Contains(m.EmployeeName))
                                                                .Where(m => m.Isactive == true).ToList());


                        if (contains is not null)
                        {
                            foreach (var coll in contains)
                            {
                                EmpModel empModel = new EmpModel();

                                empModel.Saxeli = coll.EmployeeName;
                                empModel.ShesvlisDro = DateTime.Now;
                                empModel.GacceniliSaatebi = 8;
                                decimal xelpasisaatshi = coll.Salary / 24 / 8;
                                empModel.GamosaklebiXelpasi = xelpasisaatshi * (decimal)empModel.GacceniliSaatebi;
                                empModel.EmpregisterModelId = coll.EmpregisterModelId;

                                context.Add(empModel);
                            }


                            context.SaveChanges();
                        }


                    }
                    #endregion

                    throw new Exception();
                    foreach (IGrouping<string, EmpModel> item in query)
                    {


                        string key = item.Key;
                        EmpModel empModel = new();
                        empModel.Saxeli = key;

                        List<DateTime?> shesvlalist = new();
                        List<DateTime?> gasvlalist = new();

                        foreach (var dro in item.Where(n => n.GacceniliSaatebi == null))
                        {
                            if (dro.ShesvlisDro != null && dro.WasvlisDro == null)
                            {
                                shesvlalist.Add(dro.ShesvlisDro);
                            }
                            if (dro.ShesvlisDro == null && dro.WasvlisDro != null)
                            {
                                gasvlalist.Add(dro.WasvlisDro);
                            }

                        }

                        int incomes = shesvlalist.Count;
                        if (incomes > 0)
                        {
                            //sesvala da gamossvlis raodenoba tuar udris ertmanets
                            if (incomes != gasvlalist.Count)
                            {
                                empModel.ShesvlisDro = DateTime.Now;

                                empModel.GacceniliSaatebi = 0;
                                empModel.EmpregisterModelId = context.EmpregisterModels
                                .Where(sax => sax.EmployeeName == empModel.Saxeli).FirstOrDefault().EmpregisterModelId;

                                context.Add(empModel);
                                _ = context.SaveChanges();
                                continue;
                            }

                            //pirveli shesvlis daanagariseba
                            var pirvelisesvla = shesvlalist[0].Value;
                            int indexone = stcombo.Text.IndexOf(":");
                            string subone = stcombo.Text.Substring(0, indexone+1);
                            var dagvianebiszgvari = DateTime.Parse(subone + mrgcombo.Text);
                            int daagviana = DateTime.Compare(pirvelisesvla, dagvianebiszgvari);
                            if (daagviana > 0)
                            {
                                TimeSpan timeSpan = pirvelisesvla - DateTime.Parse(stcombo.Text);
                                empModel.GacceniliSaatebi = timeSpan.TotalHours;
                            }
                            else
                            {
                                empModel.GacceniliSaatebi = 0;
                            }

                            //bolo gasvlis daanagariseba
                            var bologasvla = gasvlalist[gasvlalist.Count - 1].Value;
                            int indextwo = endcombo.Text.IndexOf(":");
                            string subtwo = endcombo.Text.Substring(0, indextwo+1);
                            var adregasvliszgvari = DateTime.Parse(subtwo + mrgcombo.Text);
                            int adregavida = DateTime.Compare(bologasvla, adregasvliszgvari);
                            if (adregavida < 0)
                            {
                                TimeSpan timeSpan = DateTime.Parse(endcombo.Text) - bologasvla;
                                empModel.GacceniliSaatebi += timeSpan.TotalHours;
                            }

                            else
                            {
                                empModel.GacceniliSaatebi = 0;
                            }


                            if (incomes > 1)
                            {
                                for (int i = 0; i < shesvlalist.Count - 1; i++)
                                {
                                    TimeSpan timeSpan = shesvlalist[i + 1].Value.Date - gasvlalist[i].Value.Date;
                                    empModel.GacceniliSaatebi += timeSpan.TotalHours;
                                }

                            }
                            var data = context.EmpregisterModels
                                .Where(sax => sax.EmployeeName == empModel.Saxeli).FirstOrDefault();

                            empModel.EmpregisterModelId = data.EmpregisterModelId;

                            decimal xelpasisaatshi = data.Salary / 24 / 8;

                            empModel.GamosaklebiXelpasi = xelpasisaatshi * (decimal)empModel.GacceniliSaatebi;

                            empModel.ShesvlisDro = DateTime.Now;
                            _ = context.Add(empModel);
                        }
                        //
                        else
                        {
                            return;

                        }

                    }
                    _ = context.SaveChanges();

                    transaction.Commit();

                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show(ex.Message + ex.Source);
                }

            });

        }

        private int shesvlacountclick;
        private bool shesvlascopeentered;


        private async void Shesvla_Click(object sender, RoutedEventArgs e)
        {
            System.Timers.Timer timer1 = new System.Timers.Timer(3500);
            timer1.Enabled = false;
            timer1.AutoReset = false;
            timer1.Elapsed += GasvlaShesvlaBrush;
            timer1.Start();
            //gasvlabrush
            inouttextbox.Foreground = Brushes.Green;
            inouttextbox.TextAlignment = TextAlignment.Center;
            inouttextbox.FontWeight = FontWeights.Bold;
            inouttextbox.Text = "IN";

            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                ShesvlisDro = DateTime.Now,
                EmpregisterModelId = context.EmpregisterModels
                .Where(sax => sax.EmployeeName == txtbox.Text).FirstOrDefault().EmpregisterModelId
            };



            shesvlacountclick++;

            if (shesvlacountclick > 1 && shesvlascopeentered == false)
            {
                shesvlascopeentered = true;
                Shesvla.Click -= Shesvla_Click;
                System.Timers.Timer timer = new System.Timers.Timer(3500);
                timer.Enabled = true;
                timer.AutoReset = false;
                timer.Elapsed += Timer_Elapsed1;
                timer.Start();
                return;
            }

            if (shesvlacountclick == 1)
            {
                System.Timers.Timer timer = new System.Timers.Timer(4000);
                timer.Enabled = true;
                timer.AutoReset = false;
                timer.Elapsed += Shesvlacountclicknull;

                timer.Start();
            }

            _ = await context.AddAsync(emp);
            _ = await context.SaveChangesAsync();
        }


        private int gasvlacountclick;
        private bool gasvlascopeentered;
        private async void Gasvla_Click_1(object sender, RoutedEventArgs e)
        {
            //timer for reset "OUT" word qfter 3.5 minut
            System.Timers.Timer timer1 = new System.Timers.Timer(3500);
            timer1.Enabled = false;
            timer1.AutoReset = false;
            timer1.Elapsed += GasvlaShesvlaBrush;
            timer1.Start();
            //gasvlabrush
            inouttextbox.Foreground = Brushes.Red;
            inouttextbox.TextAlignment = TextAlignment.Center;
            inouttextbox.FontWeight = FontWeights.Bold;
            inouttextbox.Text = "OUT";

            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                WasvlisDro = DateTime.Now,
                EmpregisterModelId = context.EmpregisterModels
                .Where(sax => sax.EmployeeName == txtbox.Text).FirstOrDefault().EmpregisterModelId
            };

            //pirveli gasvla shemtxvevashi  gasvla arunda ikos semosvlis gareshe 
            bool shemosvlaiko = (from db in context.MyProperty
                                 where db.Saxeli == emp.Saxeli &&
                                 db.ShesvlisDro.Value.Date == emp.WasvlisDro.Value.Date &&
                                 db.WasvlisDro == null

                                 select db).Any();

            if (shemosvlaiko == false)
            {

                return;
            }

            gasvlacountclick++;

            if (gasvlacountclick > 1 && gasvlascopeentered == false)
            {
                gasvlascopeentered = true;
                Gasvla.Click -= Gasvla_Click_1;
                System.Timers.Timer timer = new System.Timers.Timer(3500);
                timer.Enabled = false;
                timer.AutoReset = false;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                gasvlacountclick = 0;
                return;
            }

            if (gasvlacountclick == 1)
            {
                System.Timers.Timer timer = new System.Timers.Timer(4000);
                timer.Enabled = true;
                timer.AutoReset = false;
                timer.Elapsed += Gaslacountclicknull;

                timer.Start();
            }

            await context.AddAsync(emp);
            await context.SaveChangesAsync();

        }


        private void GasvlaShesvlaBrush(object sender, System.Timers.ElapsedEventArgs e)
        {

            //solver exception :The calling thread cannot access this object because a different thread owns it!!
            //Whenever you update your UI elements from a thread other than the main thread, you need to use this:
            Dispatcher.Invoke(() =>
                {
                    inouttextbox.Clear();
                });
        }

        private void Shesvlacountclicknull(object sender, System.Timers.ElapsedEventArgs e)
        {
            shesvlacountclick = 0;
        }

        private void Gaslacountclicknull(object sender, System.Timers.ElapsedEventArgs e)
        {
            gasvlacountclick = 0;
        }



        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Gasvla.Click += Gasvla_Click_1;
            gasvlascopeentered = false;
            gasvlacountclick = 0;

        }

        private void Timer_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
            Shesvla.Click += Shesvla_Click;
            shesvlascopeentered = false;
            shesvlacountclick = 0;
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            DateTime startdate = fromdate.SelectedDate.Value.Date;
            DateTime enddate = todate.SelectedDate.Value.Date;

            var result = from db in context.MyProperty
                         where db.ShesvlisDro.Value.Date >= startdate &&
                          db.ShesvlisDro.Value.Date <= enddate &&
                          db.GacceniliSaatebi != null
                         select new { Name = db.Saxeli, Tarigi = db.ShesvlisDro.Value.Date.ToShortDateString(), Gacdenilisaatebi = db.GacceniliSaatebi, Gamosaklebixelpasi = db.GamosaklebiXelpasi };

            if (result.Any() == false)
            {

                MessageBox.Show("ჩანაწერები არ არსებობს!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("GacdeniliSaatebi");
                var currentrow = 1;
                worksheet.Cell(currentrow, 1).Value = "სახელი";
                worksheet.Cell(currentrow, 2).Value = "თარიღი";
                worksheet.Cell(currentrow, 3).Value = "გაცდენილისაათები";
                worksheet.Cell(currentrow, 4).Value = "გამოსაკლებიხელფასი";
                foreach (var item in result)
                {
                    currentrow++;
                    worksheet.Cell(currentrow, 1).Value = item.Name;
                    worksheet.Cell(currentrow, 2).Value = item.Tarigi;
                    worksheet.Cell(currentrow, 3).Value = item.Gacdenilisaatebi;
                    worksheet.Cell(currentrow, 4).Value = item.Gamosaklebixelpasi;

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string filename = "GacdeniliSaatebi" + DateTime.Now.ToString("yyyyMMddss") + ".xlsx";
                    File.WriteAllBytes("C:\\Users\\admin\\Desktop\\" + filename, content);
                }

            }
            MessageBox.Show("რეპორტის ფაილი წარმატებით შეინახა!", "", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistrationWindow registrationWindow = new RegistrationWindow(context);
                registrationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                registrationWindow.ResizeMode = ResizeMode.NoResize;

                registrationWindow.Show();
                Hide();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            userbox.Text = GlobalCustom.CurrentUserName;

            var role = (from db in context.UserRegistrations
                        where db.UserName == userbox.Text && db.UserRole == "Admin"
                        select new { role = db.UserRole }).Any();
            if (role)
            {
                EditSalary.IsEnabled = true;
            }
            else
            {
                EditSalary.IsEnabled = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Users users = new Users(context);
            users.Show();
            Hide();
        }

        private void EditSalary_Click(object sender, RoutedEventArgs e)
        {
            EmpgridWindow empgridWindow = new EmpgridWindow(context);
            empgridWindow.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void stcombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.StartTime = stcombo.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void endcombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.EndTIme = endcombo.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void mrgcombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.Margin = mrgcombo.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
