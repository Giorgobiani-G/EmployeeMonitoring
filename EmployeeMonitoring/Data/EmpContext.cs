using EmployeeMonitoring.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Data
{
   public class EmpContext : DbContext
    {
        public EmpContext(DbContextOptions<EmpContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<EmpModel> MyProperty { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-LILFTDD\\SQLEXPRESS;Initial Catalog=EmpMonitoring;Integrated Security=True;");
        //}
    }
}
