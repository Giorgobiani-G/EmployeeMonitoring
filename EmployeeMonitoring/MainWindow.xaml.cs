using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace EmployeeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EmpContext context;


        public MainWindow(EmpContext dbcontext)
        {
            context = dbcontext;
            InitializeComponent();



            DateTime time = DateTime.Now;
            DateTime target = new DateTime(time.Year, time.Month, time.Day, 23, 32, 0);
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
            //You can use a lock object in combination with Monitor.TryEnter.
            //Only one thread at at time will be allowed into the Monitor.TryEnter block.
            //If a thread arrives here while another thread is inside, then Monitor.TryEnter returns false.

            if (Monitor.TryEnter(outputLock))
            {

                try
                {


                    var query = from db in context.MyProperty.AsEnumerable()
                                where (db.ShesvlisDro.HasValue && db.ShesvlisDro.Value.Date == DateTime.Now.Date && db.GacceniliSaatebi == null)
                                || (db.WasvlisDro.HasValue && db.WasvlisDro.Value.Date == DateTime.Now.Date && db.GacceniliSaatebi == null)

                                group db by db.Saxeli;


                    //tu chanawerebi ari gamovides metodidan
                    if (query.Any() == false)
                    {
                        return;
                    }

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

                        //sesvala da gamossvlis raodenoba tuar udris ertmanets
                        if (incomes != gasvlalist.Count)
                        {
                            empModel.ShesvlisDro = DateTime.Now;

                            empModel.GacceniliSaatebi = 0;
                            context.Add(empModel);
                            _ = context.SaveChanges();
                            return;
                        }

                        if (incomes == 1)
                        {
                            //pirveli shesvlis daanagariseba
                            var pirvelisesvla = shesvlalist[0].Value;
                            int dagvianeba = DateTime.Compare(pirvelisesvla, DateTime.Parse("16:00"));
                            if (dagvianeba > 0)
                            {
                                TimeSpan timeSpan = pirvelisesvla - new DateTime(pirvelisesvla.Year, pirvelisesvla.Month, pirvelisesvla.Day, 9, 0, 0);
                                empModel.GacceniliSaatebi = timeSpan.TotalHours;
                            }

                            //bolo gasvlis daanagariseba
                            var bologasvla = gasvlalist[gasvlalist.Count - 1].Value;
                            int adregasvla = DateTime.Compare(bologasvla, DateTime.Parse("23:46"));
                            if (adregasvla < 0)
                            {
                                TimeSpan timeSpan = new DateTime(bologasvla.Year, bologasvla.Month, bologasvla.Day, 23, 56, 0) - bologasvla;
                                empModel.GacceniliSaatebi += timeSpan.TotalHours;
                            }

                            if (incomes > 1)
                            {
                                for (int i = 0; i < shesvlalist.Count - 1; i++)
                                {
                                    TimeSpan timeSpan = shesvlalist[i + 1].Value.Date - gasvlalist[i].Value.Date;
                                    empModel.GacceniliSaatebi += timeSpan.TotalHours;
                                }

                            }

                            empModel.ShesvlisDro = DateTime.Now;
                            context.Add(empModel);
                        }
                        //
                        else
                        {
                            return;

                        }

                    }

                    _ = context.SaveChanges();


                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);

                    //}

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message +ex.Source);
                }

                finally
                {
                    Monitor.Exit(outputLock);
                }


            }





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
                ShesvlisDro = DateTime.Now

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

          

            DateTime value = emp.ShesvlisDro.Value;
            var shesvliszgvari = new DateTime(value.Year, value.Month, value.Day, 9, 0, 0);
            int adresevida = DateTime.Compare((DateTime)emp.ShesvlisDro, shesvliszgvari);
            if (adresevida < 0)
            {
                emp.ShesvlisDro = shesvliszgvari;
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
                WasvlisDro = DateTime.Now

            };

            //pirveli gasvla shemtxvevashi  gasvla arunda ikos semosvlis gareshe 
            bool shemosvlaiko = (from db in context.MyProperty
                          where db.Saxeli == emp.Saxeli && 
                          db.ShesvlisDro.Value.Date==emp.WasvlisDro.Value.Date&&
                          db.WasvlisDro==null
                          
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



            
            DateTime value = emp.WasvlisDro.Value;
            var gasvliszgvari = new DateTime(value.Year, value.Month, value.Day, 18, 0, 0);
            int gviangavida = DateTime.Compare((DateTime)emp.WasvlisDro, gasvliszgvari);
            if (gviangavida > 0)
            {
                emp.WasvlisDro = gasvliszgvari;
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





    }
}
