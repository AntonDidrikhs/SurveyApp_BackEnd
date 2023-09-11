using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class SpellingFixAndNewQuestionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Questions");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
