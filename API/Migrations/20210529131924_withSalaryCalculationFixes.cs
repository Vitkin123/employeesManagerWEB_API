using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class withSalaryCalculationFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearsOfExperience",
                table: "Employees",
                newName: "MonthsOfExperience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthsOfExperience",
                table: "Employees",
                newName: "YearsOfExperience");
        }
    }
}
