using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_xx_Relation_AppUsers_Organisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserOrganisation",
                columns: table => new
                {
                    OrganisationUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganissationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserOrganisation", x => new { x.OrganisationUsersId, x.OrganissationsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserOrganisation_AspNetUsers_OrganisationUsersId",
                        column: x => x.OrganisationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserOrganisation_Organisation_OrganissationsId",
                        column: x => x.OrganissationsId,
                        principalTable: "Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserOrganisation_OrganissationsId",
                table: "ApplicationUserOrganisation",
                column: "OrganissationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserOrganisation");
        }
    }
}
