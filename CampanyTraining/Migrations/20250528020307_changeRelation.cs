using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class changeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quiz_CourseId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Course");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfQuestions",
                table: "Quiz",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_CourseId",
                table: "Quiz",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quiz_CourseId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "NumberOfQuestions",
                table: "Quiz");

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_CourseId",
                table: "Quiz",
                column: "CourseId",
                unique: true);
        }
    }
}
