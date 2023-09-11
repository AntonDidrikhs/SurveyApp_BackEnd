using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class NewRankingOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_RankingQuestionQuestionId",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Options_RankingQuestionQuestionId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "RankingQuestionQuestionId",
                table: "Options");

            migrationBuilder.AddColumn<int>(
                name: "RankingOptionEntityRankOptionId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RankingOptionEntity",
                columns: table => new
                {
                    RankOptionId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionOrder = table.Column<int>(type: "int", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_RankingOptionEntity_RankingOptionEntityRankOptionId",
                table: "UserAnswers");

            migrationBuilder.DropTable(
                name: "RankingOptionEntity");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_RankingOptionEntityRankOptionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "RankingOptionEntityRankOptionId",
                table: "UserAnswers");

            migrationBuilder.AddColumn<int>(
                name: "RankingQuestionQuestionId",
                table: "Options",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Options_RankingQuestionQuestionId",
                table: "Options",
                column: "RankingQuestionQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_RankingQuestionQuestionId",
                table: "Options",
                column: "RankingQuestionQuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId");
        }
    }
}
