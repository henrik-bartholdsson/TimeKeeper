using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserOrganisation_AspNetUsers_OrganisationUsersId",
                table: "ApplicationUserOrganisation");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserOrganisation_Organisation_OrganissationsId",
                table: "ApplicationUserOrganisation");

            migrationBuilder.RenameColumn(
                name: "OrganissationsId",
                table: "ApplicationUserOrganisation",
                newName: "OrganisationsId");

            migrationBuilder.RenameColumn(
                name: "OrganisationUsersId",
                table: "ApplicationUserOrganisation",
                newName: "ApplicationUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserOrganisation_OrganissationsId",
                table: "ApplicationUserOrganisation",
                newName: "IX_ApplicationUserOrganisation_OrganisationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserOrganisation_AspNetUsers_ApplicationUsersId",
                table: "ApplicationUserOrganisation",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserOrganisation_Organisation_OrganisationsId",
                table: "ApplicationUserOrganisation",
                column: "OrganisationsId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserOrganisation_AspNetUsers_ApplicationUsersId",
                table: "ApplicationUserOrganisation");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserOrganisation_Organisation_OrganisationsId",
                table: "ApplicationUserOrganisation");

            migrationBuilder.RenameColumn(
                name: "OrganisationsId",
                table: "ApplicationUserOrganisation",
                newName: "OrganissationsId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUsersId",
                table: "ApplicationUserOrganisation",
                newName: "OrganisationUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserOrganisation_OrganisationsId",
                table: "ApplicationUserOrganisation",
                newName: "IX_ApplicationUserOrganisation_OrganissationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserOrganisation_AspNetUsers_OrganisationUsersId",
                table: "ApplicationUserOrganisation",
                column: "OrganisationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserOrganisation_Organisation_OrganissationsId",
                table: "ApplicationUserOrganisation",
                column: "OrganissationsId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
