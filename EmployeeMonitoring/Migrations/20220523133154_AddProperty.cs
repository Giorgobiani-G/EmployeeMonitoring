using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMonitoring.Migrations
{
    public partial class AddProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isactive",
                schema: "dbo",
                table: "Empregister",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isactive",
                schema: "dbo",
                table: "Empregister");
        }
    }
}
