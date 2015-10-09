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
    partial class HRMSMigration_v4
    {
        public override string Id
        {
            get { return "20151007111301_HRMSMigration_v4"; }
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

                    b.Property<DateTime?>("CheckIn");

                    b.Property<DateTime?>("CheckOut");

                    b.Property<double>("MinuteLate");

                    b.Property<int?>("MonthlyRecordId");

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

            modelBuilder.Entity("HRMS.Web.Models.MonthlyRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Month");

                    b.Property<double>("TotalLackTime");

                    b.Property<int>("Type");

                    b.Property<int?>("UserInfoId");

                    b.Property<int>("Year");

                    b.Key("Id");

                    b.Index("Month", "Year");
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
                    b.Reference("HRMS.Web.Models.DailyWorkingRecord")
                        .InverseCollection()
                        .ForeignKey("DailyRecordId");

                    b.Reference("HRMS.Web.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserId");
                });

            modelBuilder.Entity("HRMS.Web.Models.DailyWorkingRecord", b =>
                {
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
                });
        }
    }
}
