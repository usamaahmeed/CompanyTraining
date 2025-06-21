using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class addCourseIdForUserLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "UserLessons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_CourseId",
                table: "UserLessons",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_Course_CourseId",
                table: "UserLessons",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_Course_CourseId",
                table: "UserLessons");

            migrationBuilder.DropIndex(
                name: "IX_UserLessons_CourseId",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "UserLessons");
        }
    }
}
