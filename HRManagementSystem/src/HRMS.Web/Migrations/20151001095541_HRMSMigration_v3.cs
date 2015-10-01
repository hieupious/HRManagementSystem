using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(name: "FK_CheckInOutRecord_DailyWorkingReport_WorkingReportId", table: "CheckInOutRecord");
            //migrationBuilder.DropColumn(name: "WorkingReportId", table: "CheckInOutRecord");
            //migrationBuilder.DropTable("DailyWorkingReport");
            //migrationBuilder.DropTable("MonthlyReport");
            migrationBuilder.CreateTable(
                name: "MonthlyRecord",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Month = table.Column<int>(isNullable: false),
                    TotalLackTime = table.Column<double>(isNullable: false),
                    Type = table.Column<int>(isNullable: false),
                    UserId = table.Column<int>(isNullable: false),
                    UserInfoId = table.Column<int>(isNullable: true),
                    Year = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyRecord_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "DailyWorkingRecord",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CheckIn = table.Column<DateTime>(isNullable: true),
                    CheckOut = table.Column<DateTime>(isNullable: true),
                    MinuteLate = table.Column<double>(isNullable: false),
                    MonthlyRecordId = table.Column<int>(isNullable: false),
                    UserId = table.Column<int>(isNullable: false),
                    UserInfoId = table.Column<int>(isNullable: true),
                    WorkingDay = table.Column<DateTime>(isNullable: false),
                    WorkingType = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyWorkingRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyWorkingRecord_MonthlyRecord_MonthlyRecordId",
                        column: x => x.MonthlyRecordId,
                        principalTable: "MonthlyRecord",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyWorkingRecord_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });
            migrationBuilder.AddColumn<int>(
                name: "DailyRecordId",
                table: "CheckInOutRecord",
                isNullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_CheckInOutRecord_DailyWorkingRecord_DailyRecordId",
                table: "CheckInOutRecord",
                column: "DailyRecordId",
                principalTable: "DailyWorkingRecord",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_CheckInOutRecord_DailyWorkingRecord_DailyRecordId", table: "CheckInOutRecord");
            migrationBuilder.DropColumn(name: "DailyRecordId", table: "CheckInOutRecord");
            migrationBuilder.DropTable("DailyWorkingRecord");
            migrationBuilder.DropTable("MonthlyRecord");
            migrationBuilder.CreateTable(
                name: "MonthlyReport",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Month = table.Column<int>(isNullable: false),
                    TotalLackTime = table.Column<double>(isNullable: false),
                    User = table.Column<int>(isNullable: false),
                    UserInfoId = table.Column<int>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyReport_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "DailyWorkingReport",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CheckIn = table.Column<DateTime>(isNullable: true),
                    CheckOut = table.Column<DateTime>(isNullable: true),
                    MinuteLate = table.Column<double>(isNullable: false),
                    MonthlyReportId = table.Column<int>(isNullable: false),
                    UserId = table.Column<int>(isNullable: false),
                    UserInfoId = table.Column<int>(isNullable: true),
                    WorkingDay = table.Column<DateTime>(isNullable: false),
                    WorkingType = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyWorkingReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyWorkingReport_MonthlyReport_MonthlyReportId",
                        column: x => x.MonthlyReportId,
                        principalTable: "MonthlyReport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyWorkingReport_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });
            migrationBuilder.AddColumn<int>(
                name: "WorkingReportId",
                table: "CheckInOutRecord",
                isNullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_CheckInOutRecord_DailyWorkingReport_WorkingReportId",
                table: "CheckInOutRecord",
                column: "WorkingReportId",
                principalTable: "DailyWorkingReport",
                principalColumn: "Id");
        }
    }
}
