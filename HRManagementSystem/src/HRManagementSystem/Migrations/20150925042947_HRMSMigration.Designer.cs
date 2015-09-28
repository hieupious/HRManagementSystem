using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using HRMS.Models;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace HRMS.Migrations
{
    [DbContext(typeof(Repository))]
    partial class HRMSMigration
    {
        public override string Id
        {
            get { return "20150925042947_HRMSMigration"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("HRMS.Models.CheckInOutRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CheckTime");

                    b.Property<int>("UserId");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DeparmentId");

                    b.Property<int>("DeptId");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("Name");

                    b.Key("Id");
                });

            modelBuilder.Entity("HRMS.Models.CheckInOutRecord", b =>
                {
                    b.Reference("HRMS.Models.UserInfo")
                        .InverseCollection()
                        .ForeignKey("UserId");
                });

            modelBuilder.Entity("HRMS.Models.UserInfo", b =>
                {
                    b.Reference("HRMS.Models.Department")
                        .InverseCollection()
                        .ForeignKey("DeparmentId");
                });
        }
    }
}
