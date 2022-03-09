using EmployeeMonitoring.Data;
using EmployeeMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;



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
            DateTime target = new DateTime(time.Year, time.Month, time.Day, 23, 8, 0);
            double interval = (target - DateTime.Now).TotalMilliseconds;
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += Daangarisheba;
            timer.Enabled = false;
            timer.AutoReset = false;
            timer.Start();


        }


        private object outputLock = new object();

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
                              //where db.ShesvlisDro==DateTime.Today||db.WasvlisDro==DateTime.Today
                                group db by db.Saxeli;



                    foreach (var item in query)
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

                        if (incomes != 0)
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
                            int adregasvla = DateTime.Compare(bologasvla, DateTime.Parse("23:30"));
                            if (adregasvla < 0)
                            {
                                TimeSpan timeSpan = new DateTime(bologasvla.Year, bologasvla.Month, bologasvla.Day, 23, 55, 0) - bologasvla;
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

                        }

                    }

                    context.SaveChanges();


                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);

                    //}

                }

                finally
                {
                    Monitor.Exit(outputLock);
                }


            }





        }



        int shesvlacountclick;
        private bool shesvlascopeentered;
        private async void Shesvla_Click(object sender, RoutedEventArgs e)
        {


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
                //timer.Close();


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

            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                ShesvlisDro = DateTime.Now

            };

            DateTime value = emp.ShesvlisDro.Value;
            var shesvliszgvari = new DateTime(value.Year, value.Month, value.Day, 9, 0, 0);
            int adresevida = DateTime.Compare((DateTime)emp.ShesvlisDro, shesvliszgvari);
            if (adresevida < 0)
            {
                emp.ShesvlisDro = shesvliszgvari;
            }

            _ = await context.AddAsync(emp);
            _ = await context.SaveChangesAsync();


            #region comment
            //iko tuara gasvla: true - ki
            //var gasvla = (from db in context.MyProperty
            //              where db.Saxeli == emp.Saxeli &&
            //              db.ShesvlisDro.Value.Year == emp.ShesvlisDro.Value.Year &&
            //              db.ShesvlisDro.Value.Month == emp.ShesvlisDro.Value.Month &&
            //              db.WasvlisDro == null
            //              //db.GacceniliSaatebi != null
            //              select db).Any();

            ////09:14

            //int dagvianeba = DateTime.Compare((DateTime)emp.ShesvlisDro, DateTime.Parse("22:00"));

            ////tu daigviana

            //if (dagvianeba > 0&&gasvla==false)
            //{
            //    var variable = emp.ShesvlisDro.Value;
            //    TimeSpan timeSpan = (TimeSpan)(emp.ShesvlisDro - new DateTime(variable.Year, variable.Month, variable.Day, 9, 0, 0));
            //    emp.GacceniliSaatebi = timeSpan.TotalHours;
            //}
            #endregion

        }







        //Gasvla

        int gasvlacountclick = 0;
        private bool gasvlascopeentered;
        private async void Gasvla_Click_1(object sender, RoutedEventArgs e)
        {

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
                //timer.Close();

                gasvlacountclick = 0;
                return;
            }

            if (gasvlacountclick == 1)
            {
                System.Timers.Timer timer = new System.Timers.Timer(4000);
                timer.Enabled = true;
                timer.AutoReset = false;
                timer.Elapsed += gaslacountclicknull;

                timer.Start();

            }



            EmpModel emp = new EmpModel
            {
                Saxeli = txtbox.Text,
                WasvlisDro = DateTime.Now

            };
            DateTime value = emp.WasvlisDro.Value;
            var gasvliszgvari = new DateTime(value.Year, value.Month, value.Day, 18, 0, 0);
            int gviangavida = DateTime.Compare((DateTime)emp.WasvlisDro, gasvliszgvari);
            if (gviangavida > 0)
            {
                emp.WasvlisDro = gasvliszgvari;
            }
            await context.AddAsync(emp);
            await context.SaveChangesAsync();

            #region comment










            //var dro = emp.WasvlisDro.Value;
            //var wasvlisdrozgvari =new DateTime(dro.Year, dro.Month, dro.Day, 23, 45, 0);
            //int adregasvla = DateTime.Compare(dro,wasvlisdrozgvari);
            //if (adregasvla<=0&& gasvla==false)
            //{
            //    TimeSpan gacdenilisaatebi = new DateTime(dro.Year, dro.Month, dro.Day, 23, 50, 0) - dro;
            //    emp.GacceniliSaatebi = gacdenilisaatebi.TotalHours;
            //}


            ////shemosvlebis listi vigebt bolos
            //var lastincome = (from db in context.MyProperty
            //                       where db.Saxeli == emp.Saxeli &&
            //                       db.ShesvlisDro.Value.Year == emp.WasvlisDro.Value.Year &&
            //                       db.ShesvlisDro.Value.Month == emp.WasvlisDro.Value.Month &&
            //                        //db.GacceniliSaatebi != null&&
            //                       db.WasvlisDro == null
            //                       select db).ToArray().LastOrDefault();


            ////shemosvlis dro naklebia tuara 09:14ze
            //var shemosvlisdro = lastincome.ShesvlisDro.Value;
            //var shemosvlisdrozgvari = new DateTime(shemosvlisdro.Year, shemosvlisdro.Month, shemosvlisdro.Day, 20, 5, 0);
            //var resultshemosvla = DateTime.Compare(shemosvlisdro,shemosvlisdrozgvari);
            ////gasvlis dro naklebia tuara 17:45ze
            //var gasvlisdro = emp.WasvlisDro.Value;
            //var gasvlisdrozgvari = new DateTime(gasvlisdro.Year, gasvlisdro.Month, gasvlisdro.Day, 20,6, 0);
            //var resultgaslva = DateTime.Compare(gasvlisdro,gasvlisdrozgvari);
            ////roca shesvla da gasvla an ertia swori an meore masin....
            //if (emp.GacceniliSaatebi == null && resultshemosvla <= 0 && resultgaslva >= 0  || resultshemosvla > 0 && resultgaslva > 0 )
            //{
            //    if (emp.GacceniliSaatebi == null&& resultshemosvla <= 0&&resultgaslva<0|| resultshemosvla>0&&resultgaslva<0)
            //    {
            //        var res = new DateTime(gasvlisdro.Year, gasvlisdro.Month, gasvlisdro.Day, 21, 0, 0) - gasvlisdro;
            //       var saatebi = res.TotalHours;
            //        emp.GacceniliSaatebi = saatebi;
            //        _ = await context.AddAsync(emp);
            //        _ = await context.SaveChangesAsync();
            //        return;

            //    }
            //    _ = await context.AddAsync(emp);
            //    _ = await context.SaveChangesAsync();
            //    return;
            //}

            //if (lastincome != null)
            //{
            //    var variabel = lastincome.ShesvlisDro.Value;
            //    TimeSpan timeSpan = (TimeSpan)(emp.WasvlisDro - 
            //        new DateTime(variabel.Year, variabel.Month, variabel.Day, variabel.Hour, variabel.Minute, variabel.Second));
            //    emp.GacceniliSaatebi = timeSpan.TotalHours;
            //    _ = await context.AddAsync(emp);
            //    _ = await context.SaveChangesAsync();
            //} 
            #endregion





        }

        private void Shesvlacountclicknull(object sender, System.Timers.ElapsedEventArgs e)
        {
            shesvlacountclick = 0;
        }

        private void gaslacountclicknull(object sender, System.Timers.ElapsedEventArgs e)
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
