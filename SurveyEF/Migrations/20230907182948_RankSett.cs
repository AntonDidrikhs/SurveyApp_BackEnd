using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class RankSett : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_RankingOptionEntity_RankingOptionEntityRankOptionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_RankingOptionEntityRankOptionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RankingOptionEntityRankOptionId",
                table: "UserAnswers");

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
                name: "IX_RankingOptionEntity_RankOptionId_QuestionId",
                table: "RankingOptionEntity",
                columns: new[] { "RankOptionId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankingAnswerEntityRankingOptionEntity_RankingOptionEntityRankOptionId",
                table: "RankingAnswerEntityRankingOptionEntity",
                column: "RankingOptionEntityRankOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankingAnswerEntityRankingOptionEntity");

            migrationBuilder.DropIndex(
                name: "IX_RankingOptionEntity_RankOptionId_QuestionId",
                table: "RankingOptionEntity");

            migrationBuilder.AddColumn<int>(
                name: "RankingOptionEntityRankOptionId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_RankingOptionEntityRankOptionId",
                table: "UserAnswers",
                column: "RankingOptionEntityRankOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_RankingOptionEntity_RankingOptionEntityRankOptionId",
                table: "UserAnswers",
                column: "RankingOptionEntityRankOptionId",
                principalTable: "RankingOptionEntity",
                principalColumn: "RankOptionId");
        }
    }
}
