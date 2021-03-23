using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Renaming_FK_in_Organisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "ParentOrganisationId",
                table: "Organisation",
                newName: "FK_Parent_OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_FK_Parent_OrganisationId",
                table: "Organisation",
                column: "FK_Parent_OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisation_Organisation_FK_Parent_OrganisationId",
                table: "Organisation",
                column: "FK_Parent_OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisation_Organisation_FK_Parent_OrganisationId",
                table: "Organisation");

            migrationBuilder.DropIndex(
                name: "IX_Organisation_FK_Parent_OrganisationId",
                table: "Organisation");

            migrationBuilder.RenameColumn(
                name: "FK_Parent_OrganisationId",
                table: "Organisation",
                newName: "ParentOrganisationId");

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
    }
}
