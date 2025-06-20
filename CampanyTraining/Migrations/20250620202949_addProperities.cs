using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class addProperities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedAnswer",
                table: "UserAnswer");

            migrationBuilder.AddColumn<int>(
                name: "SelectedChoiceId",
                table: "UserAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_SelectedChoiceId",
                table: "UserAnswer",
                column: "SelectedChoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_Choices_SelectedChoiceId",
                table: "UserAnswer",
                column: "SelectedChoiceId",
                principalTable: "Choices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_Choices_SelectedChoiceId",
                table: "UserAnswer");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswer_SelectedChoiceId",
                table: "UserAnswer");

            migrationBuilder.DropColumn(
                name: "SelectedChoiceId",
                table: "UserAnswer");

            migrationBuilder.AddColumn<bool>(
                name: "SelectedAnswer",
                table: "UserAnswer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
