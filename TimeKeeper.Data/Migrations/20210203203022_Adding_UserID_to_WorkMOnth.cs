using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_UserID_to_WorkMOnth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkMonths",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkMonths");
        }
    }
}
