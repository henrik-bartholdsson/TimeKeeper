using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_sections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Organisation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_OrganisationId",
                table: "Organisation",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisation_Organisation_OrganisationId",
                table: "Organisation",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisation_Organisation_OrganisationId",
                table: "Organisation");

            migrationBuilder.DropIndex(
                name: "IX_Organisation_OrganisationId",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Organisation");
        }
    }
}
