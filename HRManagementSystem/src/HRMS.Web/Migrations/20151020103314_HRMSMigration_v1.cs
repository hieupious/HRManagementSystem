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
                name: "BaseTimeWorkingHoursRule",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    BreaktimeEnd = table.Column<TimeSpan>(isNullable: false),
                    BreaktimeStart = table.Column<TimeSpan>(isNullable: false),
                    WorkingTimeEnd = table.Column<TimeSpan>(isNullable: false),
                    WorkingTimeStart = table.Column<TimeSpan>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseTimeWorkingHoursRule", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Name = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "EarlyToleranceWorkingHoursRule",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Tolerance = table.Column<TimeSpan>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarlyToleranceWorkingHoursRule", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "LateToleranceWorkingHoursRule",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Tolerance = table.Column<TimeSpan>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LateToleranceWorkingHoursRule", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "ToleranceWorkingHoursRuleBase",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Tolerance = table.Column<TimeSpan>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToleranceWorkingHoursRuleBase", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "WorkingHoursRuleBase",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHoursRuleBase", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "WorkingPoliciesGroup",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    BaseTimeWorkingHoursRuleId = table.Column<int>(isNullable: true),
                    EarlyToleranceWorkingHoursRuleId = table.Column<int>(isNullable: true),
                    LateToleranceWorkingHoursRuleId = table.Column<int>(isNullable: true),
                    Name = table.Column<string>(isNullable: true),
                    ToleranceWorkingHoursRuleBaseId = table.Column<int>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingPoliciesGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingPoliciesGroup_BaseTimeWorkingHoursRule_BaseTimeWorkingHoursRuleId",
                        column: x => x.BaseTimeWorkingHoursRuleId,
                        principalTable: "BaseTimeWorkingHoursRule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkingPoliciesGroup_EarlyToleranceWorkingHoursRule_EarlyToleranceWorkingHoursRuleId",
                        column: x => x.EarlyToleranceWorkingHoursRuleId,
                        principalTable: "EarlyToleranceWorkingHoursRule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkingPoliciesGroup_LateToleranceWorkingHoursRule_LateToleranceWorkingHoursRuleId",
                        column: x => x.LateToleranceWorkingHoursRuleId,
                        principalTable: "LateToleranceWorkingHoursRule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkingPoliciesGroup_ToleranceWorkingHoursRuleBase_ToleranceWorkingHoursRuleBaseId",
                        column: x => x.ToleranceWorkingHoursRuleBaseId,
                        principalTable: "ToleranceWorkingHoursRuleBase",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(isNullable: false),
                    EmployeeId = table.Column<string>(isNullable: true),
                    EnglishName = table.Column<string>(isNullable: true),
                    ExternalId = table.Column<int>(isNullable: false),
                    FingerPrintId = table.Column<string>(isNullable: true),
                    Name = table.Column<string>(isNullable: true),
                    Office = table.Column<int>(isNullable: false),
                    Role = table.Column<int>(isNullable: false),
                    UserGroupId = table.Column<int>(isNullable: true),
                    WindowsAccount = table.Column<string>(isNullable: true),
                    WorkingPoliciesGroupId = table.Column<int>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                    table.UniqueConstraint("AK_UserInfo_ExternalId", x => x.ExternalId);
                    table.ForeignKey(
                        name: "FK_UserInfo_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserInfo_WorkingPoliciesGroup_WorkingPoliciesGroupId",
                        column: x => x.WorkingPoliciesGroupId,
                        principalTable: "WorkingPoliciesGroup",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "MonthlyRecord",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Month = table.Column<int>(isNullable: false),
                    TotalLackTime = table.Column<double>(isNullable: false),
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
                    Approved = table.Column<bool>(isNullable: true),
                    ApproverComment = table.Column<string>(isNullable: true),
                    ApproverId = table.Column<int>(isNullable: true),
                    CheckIn = table.Column<DateTime>(isNullable: true),
                    CheckOut = table.Column<DateTime>(isNullable: true),
                    GetApprovedReason = table.Column<string>(isNullable: true),
                    MinuteLate = table.Column<double>(isNullable: false),
                    MonthlyRecordId = table.Column<int>(isNullable: true),
                    UserInfoId = table.Column<int>(isNullable: true),
                    WorkingDay = table.Column<DateTime>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyWorkingRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyWorkingRecord_UserInfo_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
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
            migrationBuilder.CreateTable(
                name: "CheckInOutRecord",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CheckTime = table.Column<DateTime>(isNullable: false),
                    DailyRecordId = table.Column<int>(isNullable: true),
                    UserId = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInOutRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInOutRecord_DailyWorkingRecord_DailyRecordId",
                        column: x => x.DailyRecordId,
                        principalTable: "DailyWorkingRecord",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckInOutRecord_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "ExternalId");
                });
            migrationBuilder.CreateIndex(
                name: "IX_CheckInOutRecord_CheckTime",
                table: "CheckInOutRecord",
                column: "CheckTime");
            migrationBuilder.CreateIndex(
                name: "IX_CheckInOutRecord_UserId_CheckTime",
                table: "CheckInOutRecord",
                columns: new[] { "UserId", "CheckTime" });
            migrationBuilder.CreateIndex(
                name: "IX_DailyWorkingRecord_WorkingDay",
                table: "DailyWorkingRecord",
                column: "WorkingDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CheckInOutRecord");
            migrationBuilder.DropTable("WorkingHoursRuleBase");
            migrationBuilder.DropTable("DailyWorkingRecord");
            migrationBuilder.DropTable("MonthlyRecord");
            migrationBuilder.DropTable("UserInfo");
            migrationBuilder.DropTable("Department");
            migrationBuilder.DropTable("WorkingPoliciesGroup");
            migrationBuilder.DropTable("BaseTimeWorkingHoursRule");
            migrationBuilder.DropTable("EarlyToleranceWorkingHoursRule");
            migrationBuilder.DropTable("LateToleranceWorkingHoursRule");
            migrationBuilder.DropTable("ToleranceWorkingHoursRuleBase");
        }
    }
}
