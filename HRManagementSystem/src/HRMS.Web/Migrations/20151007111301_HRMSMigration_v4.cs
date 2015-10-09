using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "UserId", table: "MonthlyRecord");
            migrationBuilder.DropColumn(name: "UserId", table: "DailyWorkingRecord");
            migrationBuilder.CreateIndex(
                name: "IX_MonthlyRecord_Month_Year",
                table: "MonthlyRecord",
                columns: new[] { "Month", "Year" });
            migrationBuilder.CreateIndex(
                name: "IX_CheckInOutRecord_CheckTime",
                table: "CheckInOutRecord",
                column: "CheckTime");
            migrationBuilder.CreateIndex(
                name: "IX_CheckInOutRecord_UserId_CheckTime",
                table: "CheckInOutRecord",
                columns: new[] { "UserId", "CheckTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_MonthlyRecord_Month_Year", table: "MonthlyRecord");
            migrationBuilder.DropIndex(name: "IX_CheckInOutRecord_CheckTime", table: "CheckInOutRecord");
            migrationBuilder.DropIndex(name: "IX_CheckInOutRecord_UserId_CheckTime", table: "CheckInOutRecord");
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MonthlyRecord",
                isNullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DailyWorkingRecord",
                isNullable: false,
                defaultValue: 0);
        }
    }
}
