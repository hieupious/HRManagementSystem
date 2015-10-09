using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using HRMS.Web.Models;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Type", table: "MonthlyRecord");
            migrationBuilder.AddColumn<string>(
                name: "FingerPrintId",
                table: "UserInfo",
                isNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "UserInfo",
                isNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserInfo",
                isNullable: false,
                defaultValue: 1);
            migrationBuilder.AddColumn<string>(
                name: "WindowsAccount",
                table: "UserInfo",
                isNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "ApprovedStatus",
                table: "DailyWorkingRecord",
                isNullable: false,
                defaultValue: 1);
            migrationBuilder.AddColumn<string>(
                name: "ApproverComment",
                table: "DailyWorkingRecord",
                isNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "ApproverId",
                table: "DailyWorkingRecord",
                isNullable: true);
            migrationBuilder.AddColumn<string>(
                name: "GetApprovedReason",
                table: "DailyWorkingRecord",
                isNullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_DailyWorkingRecord_WorkingDay",
                table: "DailyWorkingRecord",
                column: "WorkingDay");
            migrationBuilder.AddForeignKey(
                name: "FK_DailyWorkingRecord_UserInfo_ApproverId",
                table: "DailyWorkingRecord",
                column: "ApproverId",
                principalTable: "UserInfo",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_UserInfo_UserInfo_ManagerId",
                table: "UserInfo",
                column: "ManagerId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_DailyWorkingRecord_UserInfo_ApproverId", table: "DailyWorkingRecord");
            migrationBuilder.DropForeignKey(name: "FK_UserInfo_UserInfo_ManagerId", table: "UserInfo");
            migrationBuilder.DropIndex(name: "IX_DailyWorkingRecord_WorkingDay", table: "DailyWorkingRecord");
            migrationBuilder.DropColumn(name: "FingerPrintId", table: "UserInfo");
            migrationBuilder.DropColumn(name: "ManagerId", table: "UserInfo");
            migrationBuilder.DropColumn(name: "Role", table: "UserInfo");
            migrationBuilder.DropColumn(name: "WindowsAccount", table: "UserInfo");
            migrationBuilder.DropColumn(name: "ApprovedStatus", table: "DailyWorkingRecord");
            migrationBuilder.DropColumn(name: "ApproverComment", table: "DailyWorkingRecord");
            migrationBuilder.DropColumn(name: "ApproverId", table: "DailyWorkingRecord");
            migrationBuilder.DropColumn(name: "GetApprovedReason", table: "DailyWorkingRecord");
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MonthlyRecord",
                isNullable: false,
                defaultValue: 0);
        }
    }
}
