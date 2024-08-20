using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Not.Again.Database.Migrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestAssembly",
                columns: table => new
                {
                    TestAssemblyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestAssemblyName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAssembly", x => x.TestAssemblyId);
                });

            migrationBuilder.CreateTable(
                name: "TestRecord",
                columns: table => new
                {
                    TestRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestAssemblyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DelimitedTestArguments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastHash = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRecord", x => x.TestRecordId);
                    table.ForeignKey(
                        name: "FK_TestRecord_TestAssembly_TestAssemblyId",
                        column: x => x.TestAssemblyId,
                        principalTable: "TestAssembly",
                        principalColumn: "TestAssemblyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestRun",
                columns: table => new
                {
                    TestRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    TotalDuration = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRun", x => x.TestRunId);
                    table.ForeignKey(
                        name: "FK_TestRun_TestRecord_TestRecordId",
                        column: x => x.TestRecordId,
                        principalTable: "TestRecord",
                        principalColumn: "TestRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestRecord_TestAssemblyId",
                table: "TestRecord",
                column: "TestAssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRun_TestRecordId",
                table: "TestRun",
                column: "TestRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestRun");

            migrationBuilder.DropTable(
                name: "TestRecord");

            migrationBuilder.DropTable(
                name: "TestAssembly");
        }
    }
}
