using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_11_WorkMonth_Organisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "WorkMonths",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkMonths_OrganisationId",
                table: "WorkMonths",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkMonths_Organisation_OrganisationId",
                table: "WorkMonths",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkMonths_Organisation_OrganisationId",
                table: "WorkMonths");

            migrationBuilder.DropIndex(
                name: "IX_WorkMonths_OrganisationId",
                table: "WorkMonths");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "WorkMonths");
        }
    }
}
