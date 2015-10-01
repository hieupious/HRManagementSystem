using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyWorkingReport",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    ClockIn = table.Column<DateTime>(isNullable: false),
                    ClockOut = table.Column<DateTime>(isNullable: false),
                    MinuteLate = table.Column<string>(isNullable: true),
                    UserId = table.Column<int>(isNullable: false),
                    WorkingDay = table.Column<DateTime>(isNullable: false),
                    WorkingType = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyWorkingReport", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false),
                    Name = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false),
                    DepartmentId = table.Column<int>(isNullable: false),
                    EmployeeId = table.Column<string>(isNullable: true),
                    Name = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInfo_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "CheckInOutRecord",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CheckTime = table.Column<DateTime>(isNullable: false),
                    UserId = table.Column<int>(isNullable: false),
                    WorkingReportId = table.Column<int>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInOutRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInOutRecord_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckInOutRecord_DailyWorkingReport_WorkingReportId",
                        column: x => x.WorkingReportId,
                        principalTable: "DailyWorkingReport",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CheckInOutRecord");
            migrationBuilder.DropTable("UserInfo");
            migrationBuilder.DropTable("DailyWorkingReport");
            migrationBuilder.DropTable("Department");
        }
    }
}