using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace HRMS.Web.Migrations
{
    public partial class HRMSMigration_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VietnamesePublicHoliday",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Description = table.Column<string>(isNullable: true),
                    IsFixed = table.Column<bool>(isNullable: false),
                    Name = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VietnamesePublicHoliday", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "PublicHoliday",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(isNullable: false),
                    VietnamesePublicHolidayId = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHoliday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicHoliday_VietnamesePublicHoliday_VietnamesePublicHolidayId",
                        column: x => x.VietnamesePublicHolidayId,
                        principalTable: "VietnamesePublicHoliday",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("PublicHoliday");
            migrationBuilder.DropTable("VietnamesePublicHoliday");
        }
    }
}
