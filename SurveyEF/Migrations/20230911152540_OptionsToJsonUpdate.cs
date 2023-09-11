using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class OptionsToJsonUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Options_OptionId",
                table: "UserAnswers");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_OptionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "UserAnswers");

            migrationBuilder.AddColumn<string>(
                name: "MultipleChoiceAnswerEntity_Answer",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RankingQuestion_OptionsJson",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultipleChoiceAnswerEntity_Answer",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RankingQuestion_OptionsJson",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionOrder = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Options_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_OptionId",
                table: "UserAnswers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionId_QuestionId",
                table: "Options",
                columns: new[] { "OptionId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Options_QuestionId",
                table: "Options",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Options_OptionId",
                table: "UserAnswers",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
