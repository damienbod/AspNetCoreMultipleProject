using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNet5MultipleProject.Migrations
{
    public partial class testMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceInfos",
                columns: table => new
                {
                    SourceInfoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceInfos", x => x.SourceInfoId);
                });

            migrationBuilder.CreateTable(
                name: "DataEventRecords",
                columns: table => new
                {
                    DataEventRecordId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SourceInfoId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEventRecords", x => x.DataEventRecordId);
                    table.ForeignKey(
                        name: "FK_DataEventRecords_SourceInfos_SourceInfoId",
                        column: x => x.SourceInfoId,
                        principalTable: "SourceInfos",
                        principalColumn: "SourceInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataEventRecords_SourceInfoId",
                table: "DataEventRecords",
                column: "SourceInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataEventRecords");

            migrationBuilder.DropTable(
                name: "SourceInfos");
        }
    }
}
