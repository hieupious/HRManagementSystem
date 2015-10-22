using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "UserGroupId", table: "UserInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserGroupId",
                table: "UserInfo",
                isNullable: true);
        }
    }
}
