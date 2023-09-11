using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyEF.Migrations
{
    /// <inheritdoc />
    public partial class AnswerTextFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "UserAnswers");
        }
    }
}
