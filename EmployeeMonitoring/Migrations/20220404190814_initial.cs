using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMonitoring.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Empregister",
                schema: "dbo",
                columns: table => new
                {
                    EmpregisterModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empregister", x => x.EmpregisterModelId);
                });

            migrationBuilder.CreateTable(
                name: "EmpMonitor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Saxeli = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShesvlisDro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WasvlisDro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GacceniliSaatebi = table.Column<double>(type: "float", nullable: true),
                    EmpregisterModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpMonitor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpMonitor_Empregister_EmpregisterModelId",
                        column: x => x.EmpregisterModelId,
                        principalSchema: "dbo",
                        principalTable: "Empregister",
                        principalColumn: "EmpregisterModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpMonitor_EmpregisterModelId",
                schema: "dbo",
                table: "EmpMonitor",
                column: "EmpregisterModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpMonitor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Empregister",
                schema: "dbo");
        }
    }
}
