using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ApprovedStatus", table: "DailyWorkingRecord");
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "DailyWorkingRecord",
                isNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Approved", table: "DailyWorkingRecord");
            migrationBuilder.AddColumn<int>(
                name: "ApprovedStatus",
                table: "DailyWorkingRecord",
                isNullable: false,
                defaultValue: 0);
        }
    }
}
