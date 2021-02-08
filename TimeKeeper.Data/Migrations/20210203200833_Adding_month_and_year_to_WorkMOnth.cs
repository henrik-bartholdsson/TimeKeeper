using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_month_and_year_to_WorkMOnth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "WorkMonths",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "WorkMonths",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "WorkMonths");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "WorkMonths");
        }
    }
}
