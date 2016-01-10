using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DataAccessPostgreSqlProvider.Migrations
{
    public partial class testPG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceInfo",
                columns: table => new
                {
                    SourceInfoId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:Serial", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceInfo", x => x.SourceInfoId);
                });
            migrationBuilder.CreateTable(
                name: "DataEventRecord",
                columns: table => new
                {
                    DataEventRecordId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:Serial", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SourceInfoId = table.Column<int>(nullable: false),
                    SourceInfoSourceInfoId = table.Column<long>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEventRecord", x => x.DataEventRecordId);
                    table.ForeignKey(
                        name: "FK_DataEventRecord_SourceInfo_SourceInfoSourceInfoId",
                        column: x => x.SourceInfoSourceInfoId,
                        principalTable: "SourceInfo",
                        principalColumn: "SourceInfoId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("DataEventRecord");
            migrationBuilder.DropTable("SourceInfo");
        }
    }
}
