using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeeper.Data.Migrations
{
    public partial class Adding_month_and_deviation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkMonths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkMonths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deviations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayInMonth = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StopTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPredefined = table.Column<bool>(type: "bit", nullable: false),
                    DeviationTypeId = table.Column<int>(type: "int", nullable: true),
                    WorkMonthId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deviations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deviations_DeviationTypes_DeviationTypeId",
                        column: x => x.DeviationTypeId,
                        principalTable: "DeviationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deviations_WorkMonths_WorkMonthId",
                        column: x => x.WorkMonthId,
                        principalTable: "WorkMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deviations_DeviationTypeId",
                table: "Deviations",
                column: "DeviationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deviations_WorkMonthId",
                table: "Deviations",
                column: "WorkMonthId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deviations");

            migrationBuilder.DropTable(
                name: "DeviationTypes");

            migrationBuilder.DropTable(
                name: "WorkMonths");
        }
    }
}
