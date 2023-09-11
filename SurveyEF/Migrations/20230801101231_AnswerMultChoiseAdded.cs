using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class AnswerMultChoiseAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Options_AnswerOptionOptionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_AnswerOptionOptionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "AnswerOptionOptionId",
                table: "UserAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_OptionId",
                table: "UserAnswers",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Options_OptionId",
                table: "UserAnswers",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Options_OptionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_OptionId",
                table: "UserAnswers");

            migrationBuilder.AddColumn<int>(
                name: "AnswerOptionOptionId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AnswerOptionOptionId",
                table: "UserAnswers",
                column: "AnswerOptionOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Options_AnswerOptionOptionId",
                table: "UserAnswers",
                column: "AnswerOptionOptionId",
                principalTable: "Options",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
