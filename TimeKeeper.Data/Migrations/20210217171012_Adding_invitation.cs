using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_invitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deviations_DeviationTypes_DeviationTypeId",
                table: "Deviations");

            migrationBuilder.DropForeignKey(
                name: "FK_Deviations_WorkMonths_WorkMonthId",
                table: "Deviations");

            migrationBuilder.AlterColumn<int>(
                name: "WorkMonthId",
                table: "Deviations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeviationTypeId",
                table: "Deviations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Deviations_DeviationTypes_DeviationTypeId",
                table: "Deviations",
                column: "DeviationTypeId",
                principalTable: "DeviationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deviations_WorkMonths_WorkMonthId",
                table: "Deviations",
                column: "WorkMonthId",
                principalTable: "WorkMonths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deviations_DeviationTypes_DeviationTypeId",
                table: "Deviations");

            migrationBuilder.DropForeignKey(
                name: "FK_Deviations_WorkMonths_WorkMonthId",
                table: "Deviations");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.AlterColumn<int>(
                name: "WorkMonthId",
                table: "Deviations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DeviationTypeId",
                table: "Deviations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Deviations_DeviationTypes_DeviationTypeId",
                table: "Deviations",
                column: "DeviationTypeId",
                principalTable: "DeviationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deviations_WorkMonths_WorkMonthId",
                table: "Deviations",
                column: "WorkMonthId",
                principalTable: "WorkMonths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
