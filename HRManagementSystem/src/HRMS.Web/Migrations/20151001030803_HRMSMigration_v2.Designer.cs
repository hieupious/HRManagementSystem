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
    partial class HRMSMigration_v2
    {
        public override string Id
        {
            get { return "20151001030803_HRMSMigration_v2"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("HRMS.Web.Models.CheckInOutRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CheckTime");

                    b.Property<int>("UserId");

                    b.Property<int?>("WorkingReportId");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.DailyWorkingReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CheckIn");

                    b.Property<DateTime?>("CheckOut");

                    b.Property<double>("MinuteLate");

                    b.Property<int>("MonthlyReportId");

                    b.Property<int>("UserId");

                    b.Property<int?>("UserInfoId");

                    b.Property<DateTime>("WorkingDay");

                    b.Property<int>("WorkingType");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.Department", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.Property<string>("Office");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.MonthlyReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Month");

                    b.Property<double>("TotalLackTime");

                    b.Property<int>("User");

                    b.Property<int?>("UserInfoId");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.UserInfo", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("Name");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Web.Models.CheckInOutRecord", b =>
                {
                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserId");

                    b.Reference("HRMS.Web.Models.DailyWorkingReport")
                        .InverseCollection()
                        .ForeignKey("WorkingReportId");
                });

            modelBuilder.Entity("HRMS.Web.Models.DailyWorkingReport", b =>
                {
                    b.Reference("HRMS.Web.Models.MonthlyReport")
                        .InverseCollection()
                        .ForeignKey("MonthlyReportId");

                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserInfoId");
                });

            modelBuilder.Entity("HRMS.Web.Models.MonthlyReport", b =>
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
                });
        }
    }
}
