using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CheckIn", table: "DailyWorkingRecord");
            migrationBuilder.DropColumn(name: "CheckOut", table: "DailyWorkingRecord");
            migrationBuilder.AddColumn<DateTime>(
                name: "StartWorkingDay",
                table: "UserInfo",
                isNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "StartWorkingDay", table: "UserInfo");
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyWorkingRecord",
                isNullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyWorkingRecord",
                isNullable: true);
        }
    }
}
