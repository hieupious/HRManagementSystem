using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ClockIn", table: "DailyWorkingReport");
            migrationBuilder.DropColumn(name: "ClockOut", table: "DailyWorkingReport");
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
            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "Department",
                isNullable: true);
            migrationBuilder.AlterColumn<double>(
                name: "MinuteLate",
                table: "DailyWorkingReport",
                isNullable: false);
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyWorkingReport",
                isNullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyWorkingReport",
                isNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "MonthlyReportId",
                table: "DailyWorkingReport",
                isNullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "DailyWorkingReport",
                isNullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_DailyWorkingReport_MonthlyReport_MonthlyReportId",
                table: "DailyWorkingReport",
                column: "MonthlyReportId",
                principalTable: "MonthlyReport",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_DailyWorkingReport_UserInfo_UserInfoId",
                table: "DailyWorkingReport",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_DailyWorkingReport_MonthlyReport_MonthlyReportId", table: "DailyWorkingReport");
            migrationBuilder.DropForeignKey(name: "FK_DailyWorkingReport_UserInfo_UserInfoId", table: "DailyWorkingReport");
            migrationBuilder.DropColumn(name: "Office", table: "Department");
            migrationBuilder.DropColumn(name: "CheckIn", table: "DailyWorkingReport");
            migrationBuilder.DropColumn(name: "CheckOut", table: "DailyWorkingReport");
            migrationBuilder.DropColumn(name: "MonthlyReportId", table: "DailyWorkingReport");
            migrationBuilder.DropColumn(name: "UserInfoId", table: "DailyWorkingReport");
            migrationBuilder.DropTable("MonthlyReport");
            migrationBuilder.AlterColumn<string>(
                name: "MinuteLate",
                table: "DailyWorkingReport",
                isNullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "ClockIn",
                table: "DailyWorkingReport",
                isNullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
                name: "ClockOut",
                table: "DailyWorkingReport",
                isNullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
