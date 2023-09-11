using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class RankingOptionToJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankingAnswerEntityRankingOptionEntity");

            migrationBuilder.DropTable(
                name: "RankingOptionEntity");

            migrationBuilder.AddColumn<string>(
                name: "OptionsJson",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionsJson",
                table: "Questions");

            migrationBuilder.CreateTable(
                name: "RankingOptionEntity",
                columns: table => new
                {
                    RankOptionId = table.Column<int>(type: "int", nullable: false),
                    OptionOrder = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingOptionEntity", x => x.RankOptionId);
                    table.ForeignKey(
                        name: "FK_RankingOptionEntity_Questions_RankOptionId",
                        column: x => x.RankOptionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankingAnswerEntityRankingOptionEntity",
                columns: table => new
                {
                    AnswersAnswerId = table.Column<int>(type: "int", nullable: false),
                    RankingOptionEntityRankOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingAnswerEntityRankingOptionEntity", x => new { x.AnswersAnswerId, x.RankingOptionEntityRankOptionId });
                    table.ForeignKey(
                        name: "FK_RankingAnswerEntityRankingOptionEntity_RankingOptionEntity_RankingOptionEntityRankOptionId",
                        column: x => x.RankingOptionEntityRankOptionId,
                        principalTable: "RankingOptionEntity",
                        principalColumn: "RankOptionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RankingAnswerEntityRankingOptionEntity_UserAnswers_AnswersAnswerId",
                        column: x => x.AnswersAnswerId,
                        principalTable: "UserAnswers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RankingAnswerEntityRankingOptionEntity_RankingOptionEntityRankOptionId",
                table: "RankingAnswerEntityRankingOptionEntity",
                column: "RankingOptionEntityRankOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingOptionEntity_RankOptionId_QuestionId",
                table: "RankingOptionEntity",
                columns: new[] { "RankOptionId", "QuestionId" },
                unique: true);
        }
    }
}
