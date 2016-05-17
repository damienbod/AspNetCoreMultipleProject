using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet5MultipleProject.Migrations
{
    public partial class DataAccessPostgreSqlProvider : Migration
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
                    SourceInfoId1 = table.Column<long>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEventRecord", x => x.DataEventRecordId);
                    table.ForeignKey(
                        name: "FK_DataEventRecord_SourceInfo_SourceInfoId1",
                        column: x => x.SourceInfoId1,
                        principalTable: "SourceInfo",
                        principalColumn: "SourceInfoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataEventRecord_SourceInfoId1",
                table: "DataEventRecord",
                column: "SourceInfoId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataEventRecord");

            migrationBuilder.DropTable(
                name: "SourceInfo");
        }
    }
}
