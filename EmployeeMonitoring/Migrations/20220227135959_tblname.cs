using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMonitoring.Migrations
{
    public partial class tblname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "MyProperty",
                newName: "EmpMonitor",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmpMonitor",
                schema: "dbo",
                table: "EmpMonitor",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmpMonitor",
                schema: "dbo",
                table: "EmpMonitor");

            migrationBuilder.RenameTable(
                name: "EmpMonitor",
                schema: "dbo",
                newName: "MyProperty");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty",
                column: "Id");
        }
    }
}
