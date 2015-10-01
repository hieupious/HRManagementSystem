using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Web.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<CheckInOutRecord> CheckInOutRecords { get; set; }
        public DbSet<DailyWorkingRecord> DailyWorkingRecords { get; set; }
        public DbSet<MonthlyRecord> MonthlyRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserInfo>().Key(u => u.Id);
            modelBuilder.Entity<UserInfo>().Property(u => u.Id).ValueGeneratedNever();
            modelBuilder.Entity<Department>().Key(d => d.Id);
            modelBuilder.Entity<Department>().Property(d => d.Id).ValueGeneratedNever();
            modelBuilder.Entity<CheckInOutRecord>().Key(c => c.Id);
            modelBuilder.Entity<DailyWorkingRecord>().Key(d => d.Id);
            //modelBuilder.Entity<CheckInOutRecord>().Index(c => c.CheckTime);
            //modelBuilder.Entity<CheckInOutRecord>().Index(c => new { c.UserId, c.CheckTime });
        }
    }
}