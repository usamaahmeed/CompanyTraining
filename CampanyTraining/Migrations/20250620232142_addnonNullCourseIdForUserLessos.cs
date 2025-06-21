using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTraining.Migrations
{
    /// <inheritdoc />
    public partial class addnonNullCourseIdForUserLessos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_Course_CourseId",
                table: "UserLessons");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "UserLessons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_Course_CourseId",
                table: "UserLessons",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_Course_CourseId",
                table: "UserLessons");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "UserLessons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_Course_CourseId",
                table: "UserLessons",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
