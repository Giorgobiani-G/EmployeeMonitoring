using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMonitoring.Migrations
{
    public partial class doublegacdeilisaatebi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "GacceniliSaatebi",
                schema: "dbo",
                table: "EmpMonitor",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GacceniliSaatebi",
                schema: "dbo",
                table: "EmpMonitor",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
