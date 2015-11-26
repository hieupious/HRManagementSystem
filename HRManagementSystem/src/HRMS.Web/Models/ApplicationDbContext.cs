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

        public DbSet<WorkingPoliciesGroup> WorkingPoliciesGroups { get; set; }
        public DbSet<WorkingHoursRuleBase> WorkingHoursRuleBases { get; set; }
        public DbSet<BaseTimeWorkingHoursRule> BaseTimeWorkingHoursRules { get; set; }
        public DbSet<ToleranceWorkingHoursRuleBase> ToleranceWorkingHoursRuleBases { get; set; }
        public DbSet<LateToleranceWorkingHoursRule> LateToleranceWorkingHoursRules { get; set; }
        public DbSet<EarlyToleranceWorkingHoursRule> EarlyToleranceWorkingHoursRules { get; set; }

        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<VietnamesePublicHoliday> VietnamesePublicHolidays { get; set; }

        public DbSet<WorkingPoliciesGroup> GetWorkingPoliciesGroups()
        {
            foreach (var userGroup in WorkingPoliciesGroups)
            {
                var baseTime = BaseTimeWorkingHoursRules.Include(b => b.WorkingPoliciesGroups).SingleOrDefault(b => b.WorkingPoliciesGroups.Contains(userGroup));
                if (baseTime != null && userGroup.WorkingHoursRules.Count(s => s is BaseTimeWorkingHoursRule) < 1)
                    userGroup.WorkingHoursRules.Add(baseTime);
                var earlyTolerance = EarlyToleranceWorkingHoursRules.Include(e => e.WorkingPoliciesGroups).SingleOrDefault(e => e.WorkingPoliciesGroups.Contains(userGroup));
                if (earlyTolerance != null && userGroup.WorkingHoursRules.Count(s => s is EarlyToleranceWorkingHoursRule) < 1)
                    userGroup.WorkingHoursRules.Add(earlyTolerance);
                var lateTolerance = LateToleranceWorkingHoursRules.Include(l => l.WorkingPoliciesGroups).SingleOrDefault(l => l.WorkingPoliciesGroups.Contains(userGroup));
                if (lateTolerance != null && userGroup.WorkingHoursRules.Count(s => s is LateToleranceWorkingHoursRule) < 1)
                    userGroup.WorkingHoursRules.Add(lateTolerance);
            }
            return WorkingPoliciesGroups;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure foreign key
            modelBuilder.Entity<DailyWorkingRecord>().Reference(d => d.Approver).InverseCollection(u => u.Approvals).ForeignKey(d => d.ApproverId);
            modelBuilder.Entity<DailyWorkingRecord>().Reference(d => d.UserInfo).InverseCollection(u => u.DailyRecords).ForeignKey(d => d.UserInfoId);
            modelBuilder.Entity<DailyWorkingRecord>().Reference(d => d.MonthlyRecord).InverseCollection(m => m.DailyRecords).ForeignKey(d => d.MonthlyRecordId);
            modelBuilder.Entity<CheckInOutRecord>().Reference(c => c.User).InverseCollection(u => u.CheckInOutRecords).ForeignKey(c => c.UserId).PrincipalKey(u => u.ExternalId);
            modelBuilder.Entity<CheckInOutRecord>().Reference(c => c.DailyRecord).InverseCollection(d => d.CheckInOutRecords).ForeignKey(c => c.DailyRecordId);
            modelBuilder.Entity<PublicHoliday>().Reference(p => p.VNPublicHoliday).InverseCollection(v => v.PublicHolidays).ForeignKey(p => p.VietnamesePublicHolidayId);
            // Create index
            modelBuilder.Entity<CheckInOutRecord>().Index(c => c.CheckTime);
            modelBuilder.Entity<CheckInOutRecord>().Index(c => new { c.UserId, c.CheckTime });
            modelBuilder.Entity<DailyWorkingRecord>().Index(d => d.WorkingDay);

            // ignore property
            modelBuilder.Entity<DailyWorkingRecord>().Ignore(d => d.CheckIn);
            modelBuilder.Entity<DailyWorkingRecord>().Ignore(d => d.CheckOut);
        }
    }
}