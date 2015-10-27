using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using HRMS.Web.Models;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace HRMS.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class HRMSMigration_first
    {
        public override string Id
        {
            get { return "20151022093147_HRMSMigration_first"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("HRMS.Web.Models.BaseTimeWorkingHoursRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("BreaktimeEnd");

                    b.Property<TimeSpan>("BreaktimeStart");

                    b.Property<TimeSpan>("WorkingTimeEnd");

                    b.Property<TimeSpan>("WorkingTimeStart");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.CheckInOutRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CheckTime");

                    b.Property<int?>("DailyRecordId");

                    b.Property<int>("UserId");

                    b.Key("Id");

                    b.Index("CheckTime");

                    b.Index("UserId", "CheckTime");
                });

            modelBuilder.Entity("HRMS.Web.Models.DailyWorkingRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("Approved");

                    b.Property<string>("ApproverComment");

                    b.Property<int?>("ApproverId");

                    b.Property<string>("GetApprovedReason");

                    b.Property<double>("MinuteLate");

                    b.Property<int?>("MonthlyRecordId");

                    b.Property<int?>("UserInfoId");

                    b.Property<DateTime>("WorkingDay");

                    b.Key("Id");

                    b.Index("WorkingDay");
                });

            modelBuilder.Entity("HRMS.Web.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.EarlyToleranceWorkingHoursRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("Tolerance");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.LateToleranceWorkingHoursRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("Tolerance");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.MonthlyRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Month");

                    b.Property<double>("TotalLackTime");

                    b.Property<int?>("UserInfoId");

                    b.Property<int>("Year");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.ToleranceWorkingHoursRuleBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("Tolerance");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("EnglishName");

                    b.Property<int>("ExternalId");

                    b.Property<string>("FingerPrintId");

                    b.Property<string>("Name");

                    b.Property<int>("Office");

                    b.Property<int>("Role");

                    b.Property<DateTime?>("StartWorkingDay");

                    b.Property<string>("WindowsAccount");

                    b.Property<int?>("WorkingPoliciesGroupId");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.WorkingHoursRuleBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.WorkingPoliciesGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BaseTimeWorkingHoursRuleId");

                    b.Property<int?>("EarlyToleranceWorkingHoursRuleId");

                    b.Property<int?>("LateToleranceWorkingHoursRuleId");

                    b.Property<string>("Name");

                    b.Property<int?>("ToleranceWorkingHoursRuleBaseId");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.CheckInOutRecord", b =>
                {
                    b.Reference("HRMS.Web.Models.DailyWorkingRecord")
                        .InverseCollection()
                        .ForeignKey("DailyRecordId");

                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserId")
                        .PrincipalKey("ExternalId");
                });

            modelBuilder.Entity("HRMS.Web.Models.DailyWorkingRecord", b =>
                {
                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("ApproverId");

                    b.Reference("HRMS.Web.Models.MonthlyRecord")
                        .InverseCollection()
                        .ForeignKey("MonthlyRecordId");

                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserInfoId");
                });

            modelBuilder.Entity("HRMS.Web.Models.MonthlyRecord", b =>
                {
                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserInfoId");
                });

            modelBuilder.Entity("HRMS.Web.Models.UserInfo", b =>
                {
                    b.Reference("HRMS.Web.Models.Department")
                        .InverseCollection()
                        .ForeignKey("DepartmentId");

                    b.Reference("HRMS.Web.Models.WorkingPoliciesGroup")
                        .InverseCollection()
                        .ForeignKey("WorkingPoliciesGroupId");
                });

            modelBuilder.Entity("HRMS.Web.Models.WorkingPoliciesGroup", b =>
                {
                    b.Reference("HRMS.Web.Models.BaseTimeWorkingHoursRule")
                        .InverseCollection()
                        .ForeignKey("BaseTimeWorkingHoursRuleId");

                    b.Reference("HRMS.Web.Models.EarlyToleranceWorkingHoursRule")
                        .InverseCollection()
                        .ForeignKey("EarlyToleranceWorkingHoursRuleId");

                    b.Reference("HRMS.Web.Models.LateToleranceWorkingHoursRule")
                        .InverseCollection()
                        .ForeignKey("LateToleranceWorkingHoursRuleId");

                    b.Reference("HRMS.Web.Models.ToleranceWorkingHoursRuleBase")
                        .InverseCollection()
                        .ForeignKey("ToleranceWorkingHoursRuleBaseId");
                });
        }
    }
}
